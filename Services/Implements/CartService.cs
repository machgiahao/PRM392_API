using AutoMapper;
using Repositories.Entities;
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

        public CartService(ICartRepository cartRepository,
                         IProductRepository productRepository,
                         IUnitOfWork unitOfWork,
                         IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddToCartAsync(int userId, AddToCartDto item)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            var cart = await _cartRepository.GetOrCreateActiveCartByUserIdAsync(userId);

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += item.Quantity;
                cartItem.Price = product.Price * cartItem.Quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price * item.Quantity,
                    CartId = cart.CartId
                };
                await _cartRepository.AddItemToCartAsync(cart.CartId, cartItem);
            }

            cart.TotalPrice = cart.CartItems.Sum(ci => ci.Price);
            await _cartRepository.UpdateCartAsync(cart);
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
                // Logic ĐÚNG: Cập nhật số lượng, lưu ĐƠN GIÁ
                existingItem.Quantity += itemDto.Quantity;
                existingItem.Price = product.Price;
                _cartRepository.UpdateCartItem(existingItem);
            }
            else
            {
                // Logic ĐÚNG: Tạo mới, lưu ĐƠN GIÁ
                var newItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = product.Price
                };
                _cartRepository.AddCartItem(newItem);
            }

            // Tính toán lại tổng tiền (dùng hàm đã tối ưu)
            RecalculateCartTotal(cart);

            // Lưu 1 lần duy nhất
            await _unitOfWork.SaveChangesAsync();

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
