using Repositories.Entities;

namespace Repositories.Repositories.Interfaces;

public interface ICartRepository
{
    Task AddItemToCartAsync(int cartId, CartItem item);
    Task UpdateCartAsync(Cart cart);

    Task<Cart> GetOrCreateActiveCartByUserIdAsync(int userId);
    Task<List<CartItem>> GetCartItemsAsync(int cartId);
    Task<CartItem> GetCartItemAsync(int cartId, int productId);
    void AddCartItem(CartItem item);
    void UpdateCartItem(CartItem item);
    void UpdateCart(Cart cart);
    void RemoveCartItem(CartItem item);
    Task ClearAllItemsAsync(int cartId);
}
