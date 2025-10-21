using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly SalesAppDbContext _dbContext;

        public CartRepository(SalesAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cart> GetOrCreateCartByUserIdAsync(int userId)
        {
            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Status = "Active",
                    TotalPrice = 0
                };
                await _dbContext.Carts.AddAsync(cart);
                await _dbContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddItemToCartAsync(int cartId, CartItem item)
        {
            item.CartId = cartId;
            await _dbContext.CartItems.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _dbContext.Carts.Update(cart);
            await _dbContext.SaveChangesAsync();
        }
    }

}
