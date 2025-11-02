using Repositories.Entities;

namespace Repositories.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<Order?> GetOrderDetailAsync(int orderId, CancellationToken cancellationToken = default);
    }
}
