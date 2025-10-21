using Services.Dtos;

namespace Services.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(int userId, AddToCartDto item);
    }

}
