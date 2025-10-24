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

        public async Task<Cart> GetOrCreateActiveCartByUserIdAsync(int userId)
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
                    TotalPrice = 0,
                    CartItems = new List<CartItem>()
                };
                _dbContext.Carts.Add(cart);
                await _dbContext.SaveChangesAsync();
            }
            return cart;
        }

        public async Task<List<CartItem>> GetCartItemsAsync(int cartId)
        {
            return await _dbContext.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }

        public async Task<CartItem> GetCartItemAsync(int cartId, int productId)
        {
            return await _dbContext.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
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

        public void AddCartItem(CartItem item)
        {
            _dbContext.CartItems.Add(item);
        }

        public void UpdateCartItem(CartItem item)
        {
            _dbContext.CartItems.Update(item);
        }

        public void UpdateCart(Cart cart)
        {
            _dbContext.Carts.Update(cart);
        }

        public void RemoveCartItem(CartItem item)
        {
            _dbContext.CartItems.Remove(item);
        }

        public async Task ClearAllItemsAsync(int cartId)
        {
            var items = await GetCartItemsAsync(cartId);
            if (items.Any())
            {
                _dbContext.CartItems.RemoveRange(items);
            }
        }
    }

}
