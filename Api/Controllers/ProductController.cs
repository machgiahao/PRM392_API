using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProducts(
    [FromQuery] string[] sortBy,
    [FromQuery] int? categoryId,
    [FromQuery] string? brand,
    [FromQuery] decimal? minPrice,
    [FromQuery] decimal? maxPrice,
    [FromQuery] double? minRating,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        try
        {
            var pagedResult = await _productService.GetProductsAsync(sortBy, categoryId, brand, minPrice, maxPrice, minRating, pageNumber, pageSize);
            return Ok(pagedResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error!");
        }
    }
        
    [HttpGet("{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductDetail(int productId)
    {
        try
        {
            var productDetail = await _productService.GetProductDetailAsync(productId);
            return Ok(productDetail);
        } catch (Exception ex)
        {
            return StatusCode(500, "Internal server error!");
        }
    }
}
