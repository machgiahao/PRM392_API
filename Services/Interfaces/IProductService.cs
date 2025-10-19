using Services.Dtos;
using Services.Paging;

namespace Services.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetProductsAsync(
            string[] sortBy,
            int? categoryId,
            string brand,
            decimal? minPrice,
            decimal? maxPrice,
            double? minRating,
            int pageNumber,
            int pageSize);

    Task<ProductDetailDto> GetProductDetailAsync(int productId);
}
