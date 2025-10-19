using Repositories.Entities;

namespace Repositories.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<(IEnumerable<Product> Items, int TotalCount)> GetProductsAsync(
        string[] sortBy,
        int? categoryId,
        string? brand,
        decimal? minPrice,
        decimal? maxPrice,
        double? minRating,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Product> GetProductDetailAsync(int productId, CancellationToken cancellationToken = default);
}
