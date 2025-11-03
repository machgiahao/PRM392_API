using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly IPayPalService _paypal;

        public PayPalController(IPayPalService paypal)
        {
            _paypal = paypal;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var approvalUrl = await _paypal.CreateOrderAsync(request.Amount);
            return Ok(new { approvalUrl });
        }

        [HttpPost("capture-order")]
        public async Task<IActionResult> CaptureOrder([FromBody] CaptureOrderRequest request)
        {
            var success = await _paypal.CaptureOrderAsync(request.OrderId);
            return Ok(new { success });
        }
    }
}
