using AutoMapper;
using Repositories.Entities;
using Repositories.Repositories;
using Repositories.Repositories.Interfaces;
using Repositories.Uow;
using Services.Dtos;
using Services.Interfaces;

namespace Services.Implements
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public CartService(ICartRepository cartRepository,
                         IProductRepository productRepository,
                         IUnitOfWork unitOfWork,
                         IMapper mapper,
                         INotificationService notificationService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<CartDto> AddItemToCartAsync(int userId, UpdateCartItemDto itemDto)
        {
            var cart = await _cartRepository.GetOrCreateActiveCartByUserIdAsync(userId);

            var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var existingItem = await _cartRepository.GetCartItemAsync(cart.CartId, itemDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += itemDto.Quantity;
                existingItem.Price = product.Price;
                _cartRepository.UpdateCartItem(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = product.Price
                };
                _cartRepository.AddCartItem(newItem);
                cart.CartItems.Add(newItem);
            }

            RecalculateCartTotal(cart);

            await _unitOfWork.SaveChangesAsync();
            try
            {
                int totalItemsInCart = cart.CartItems.Sum(ci => ci.Quantity);

                await _notificationService.SendCartUpdatePush(userId, totalItemsInCart);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send push notification: {ex.Message}");
            }
            return await GetCartByUserIdAsync(userId);
        }

        public async Task<CartDto> UpdateItemQuantityAsync(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return await RemoveItemFromCartAsync(userId, productId);
            }

            var cart = await _cartRepository.GetOrCreateActiveCartByUserIdAsync(userId);
            var itemToUpdate = await _cartRepository.GetCartItemAsync(cart.CartId, productId);
            var product = await _productRepository.GetByIdAsync(productId);

            if (itemToUpdate != null && product != null)
            {
                itemToUpdate.Quantity = quantity;
                itemToUpdate.Price = product.Price;
                _cartRepository.UpdateCartItem(itemToUpdate);

                RecalculateCartTotal(cart);
                await _unitOfWork.SaveChangesAsync();
                try
                {
                    int totalItemsInCart = cart.CartItems.Sum(ci => ci.Quantity);

                    await _notificationService.SendCartUpdatePush(userId, totalItemsInCart);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send push notification: {ex.Message}");
                }
            }

            return await GetCartByUserIdAsync(userId);
        }

        public async Task<CartDto> RemoveItemFromCartAsync(int userId, int productId)
        {
            var cart = await _cartRepository.GetOrCreateActiveCartByUserIdAsync(userId);
            var itemToRemove = await _cartRepository.GetCartItemAsync(cart.CartId, productId);

            if (itemToRemove != null)
            {
                _cartRepository.RemoveCartItem(itemToRemove);

                cart.CartItems.Remove(itemToRemove);

                RecalculateCartTotal(cart);
                await _unitOfWork.SaveChangesAsync();
                try
                {
                    int totalItemsInCart = cart.CartItems.Sum(ci => ci.Quantity);

                    await _notificationService.SendCartUpdatePush(userId, totalItemsInCart);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send push notification: {ex.Message}");
                }
            }

            return await GetCartByUserIdAsync(userId);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetOrCreateActiveCartByUserIdAsync(userId);

            await _cartRepository.ClearAllItemsAsync(cart.CartId);
            cart.TotalPrice = 0;
            _cartRepository.UpdateCart(cart);

            await _unitOfWork.SaveChangesAsync();
            try
            {
                int totalItemsInCart = 0;

                await _notificationService.SendCartUpdatePush(userId, totalItemsInCart);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send push notification: {ex.Message}");
            }
        }

        public async Task<CartDto> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetOrCreateActiveCartByUserIdAsync(userId);

            var cartDto = _mapper.Map<CartDto>(cart);
            
            if (cart.CartItems.Any())
            {
                var productIds = cart.CartItems.Select(ci => ci.ProductId.Value).ToList();
                var products = await _productRepository.GetProductsByIdsAsync(productIds);

                foreach (var item in cart.CartItems)
                {
                    var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                    if (product == null) continue;

                    cartDto.Items.Add(new CartItemDto
                    {
                        ProductId = item.ProductId.Value,
                        ProductName = product.ProductName,
                        ImageUrl = product.ImageUrl,
                        PricePerItem = item.Price, 
                        Quantity = item.Quantity,
                        TotalItemPrice = item.Price * item.Quantity 
                    });
                }
            }

            return cartDto;
        }

        private void RecalculateCartTotal(Cart cart)
        {
            cart.TotalPrice = cart.CartItems.Sum(item => item.Quantity * item.Price);

            _cartRepository.UpdateCart(cart);
        }
    }
}
