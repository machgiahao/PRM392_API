using Services.Dtos;

namespace Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartByUserIdAsync(int userId);
        Task<CartDto> AddItemToCartAsync(int userId, UpdateCartItemDto itemDto);
        Task<CartDto> UpdateItemQuantityAsync(int userId, int productId, int quantity);
        Task<CartDto> RemoveItemFromCartAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }
}
