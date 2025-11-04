using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromToken();

                var newOrder = await _orderService.CreateOrderFromCartAsync(
                    userId,
                    request.PaymentMethod,
                    request.BillingAddress,
                    cancellationToken);

                return Ok(newOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Have (an) errors when create order." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders(CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var ordersDto = await _orderService.GetUserOrdersAsync(userId, cancellationToken);
                return Ok(ordersDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Have (an) errors when get list order of this user." });
            }
        }

        [HttpGet("order-detail/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var orderDto = await _orderService.GetOrderDetailsAsync(orderId, cancellationToken);

                if (orderDto == null)
                {
                    return NotFound(new { message = "Can not found this order." });
                }

                if (orderDto.UserId != userId)
                {
                    return Forbid();
                }

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Have (an) errors when get this order detail." });
            }
        }

        [HttpPatch("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto request, CancellationToken cancellationToken)
        {
            try
            {

                var success = await _orderService.UpdateOrderStatusAsync(orderId, request.NewStatus, cancellationToken);

                if (!success)
                {
                    return NotFound(new { message = "Can not found this order to update." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Have (an) errors when update order status." });
            }
        }

        private int GetUserIdFromToken()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("Can not found this user.");
            }

            return int.Parse(userIdString);
        }
    }
}
