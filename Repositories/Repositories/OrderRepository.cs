using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly SalesAppDbContext _dbContext;

        public OrderRepository(SalesAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await GetQueryable()
                .Where(o => o.UserId == userId)
                .Include(o => o.Cart)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetOrderDetailAsync(int orderId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .Where(o => o.OrderId == orderId)
                .Include(o => o.User)
                .Include(o => o.Cart)
                    .ThenInclude(c => c!.CartItems)
                        .ThenInclude(ci => ci.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
