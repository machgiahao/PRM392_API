using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize(Roles ="User")]
        [HttpPost("add-to-cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            await _cartService.AddToCartAsync(userId, dto);
            return Ok("Product added to cart");
        }

        [HttpGet]
        [ProducesResponseType(typeof(CartDto), 200)]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetCurrentUserId();
            var cartDto = await _cartService.GetCartByUserIdAsync(userId);
            return Ok(cartDto);
        }

        [HttpPut("item/{productId}")]
        [ProducesResponseType(typeof(CartDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateItemQuantity(int productId, [FromBody] UpdateQuantityDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var cartDto = await _cartService.UpdateItemQuantityAsync(userId, productId, dto.Quantity);
                return Ok(cartDto);
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("item/{productId}")]
        [ProducesResponseType(typeof(CartDto), 200)]
        public async Task<IActionResult> RemoveItemFromCart(int productId)
        {
            var userId = GetCurrentUserId();
            var cartDto = await _cartService.RemoveItemFromCartAsync(userId, productId);
            return Ok(cartDto);
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            await _cartService.ClearCartAsync(userId);

            return NoContent();
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            return userId;
        }
    }
}
