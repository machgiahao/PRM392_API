using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Repositories.Interfaces;

namespace Repositories.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly SalesAppDbContext _dbContext;

    public ProductRepository(SalesAppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetProductsAsync(string[] sortBy, int? categoryId, string? brand, decimal? minPrice, decimal? maxPrice, double? minRating, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = GetQueryable().Include(p => p.Category);

        //filter
        if (categoryId.HasValue && categoryId > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(p => p.Brand.ToLower() == brand.ToLower());
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice);
        }

        if (minRating.HasValue)
        {
            query = query.Where(p => p.Rating >= minRating);
        }

        //sort
        if (sortBy != null && sortBy.Length > 0)
        {
            IOrderedQueryable<Product> orderedQuery = null;

            // Apply the first sort criteria
            var firstSort = sortBy[0]?.ToLower();
            orderedQuery = firstSort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "popularity_desc" => query.OrderByDescending(p => p.SoldCount),
                "rating_desc" => query.OrderByDescending(p => p.Rating),
                _ => query.OrderBy(p => p.ProductName)
            };

            // Apply subsequent sort criteria
            for (int i = 1; i < sortBy.Length; i++)
            {
                var thenSort = sortBy[i]?.ToLower();
                orderedQuery = thenSort switch
                {
                    "price_asc" => orderedQuery.ThenBy(p => p.Price),
                    "price_desc" => orderedQuery.ThenByDescending(p => p.Price),
                    "popularity_desc" => orderedQuery.ThenByDescending(p => p.SoldCount),
                    "rating_desc" => orderedQuery.ThenByDescending(p => p.Rating),
                    _ => orderedQuery.ThenBy(p => p.ProductName)
                };
            }
            query = orderedQuery;
        }
        else
        {
            // default
            query = query.OrderBy(p => p.ProductName);
        }
        var totalCount = await query.CountAsync(cancellationToken);

        // Áp dụng phân trang
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<Product> GetProductDetailAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await GetFirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken, p => p.Category);
    }

    public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
    {
        if (productIds == null || !productIds.Any())
        {
            return new List<Product>();
        }
        return await _dbContext.Products
            .Where(p => productIds.Contains(p.ProductId))
            .ToListAsync();
    }
}
