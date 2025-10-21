using Repositories.Entities;

namespace Repositories.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetOrCreateCartByUserIdAsync(int userId);
        Task AddItemToCartAsync(int cartId, CartItem item);
        Task UpdateCartAsync(Cart cart);
    }

}
