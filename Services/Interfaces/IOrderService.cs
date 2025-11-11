using Repositories.Entities;
using Services.Dtos;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateOrderFromCartAsync(int userId, string paymentMethod, string billingAddress, CancellationToken cancellationToken = default);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default);
        Task<OrderDetailDto?> GetOrderDetailsAsync(int orderId, CancellationToken cancellationToken = default);
        Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus, CancellationToken cancellationToken = default);
    }
}
