using AutoMapper;
using Repositories.Repositories.Interfaces;
using Repositories.Uow;
using Services.Dtos;
using Services.Interfaces;
using Services.Paging;

namespace Services.Implements;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductDto>> GetProductsAsync(
            string[] sortBy,
            int? categoryId,
            string brand,
            decimal? minPrice,
            decimal? maxPrice,
            double? minRating,
            int pageNumber,
            int pageSize)
    {
        // 1. Gọi repository để lấy dữ liệu đã phân trang
        var (products, totalCount) = await _productRepository.GetProductsAsync(
            sortBy, categoryId, brand, minPrice, maxPrice, minRating, pageNumber, pageSize);

        // 2. Map danh sách sản phẩm sang DTO
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

        // 3. Tạo và trả về đối tượng PagedResult
        return new PagedResult<ProductDto>(productDtos, pageNumber, pageSize, totalCount);
    }
}
