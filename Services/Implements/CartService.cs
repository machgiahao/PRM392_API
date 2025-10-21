using Repositories.Entities;
using Repositories.Repositories.Interfaces;
using Services.Dtos;
using Services.Interfaces;

namespace Services.Implements
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task AddToCartAsync(int userId, AddToCartDto item)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            var cart = await _cartRepository.GetOrCreateCartByUserIdAsync(userId);

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

    }

}
