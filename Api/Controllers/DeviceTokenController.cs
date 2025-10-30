using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/devices")]
[Authorize(Roles = "User")]
public class DeviceTokenController : ControllerBase
{
    private readonly IDeviceTokenService _deviceTokenService;

    public DeviceTokenController(IDeviceTokenService deviceTokenService)
    {
        _deviceTokenService = deviceTokenService;
    }

    // Lấy UserId từ Token JWT
    private int GetCurrentUserId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    [HttpPost("register")]
    public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceDto dto)
    {
        if (string.IsNullOrEmpty(dto.DeviceToken))
            return BadRequest("Token is required.");

        await _deviceTokenService.RegisterDeviceAsync(GetCurrentUserId(), dto.DeviceToken);
        return Ok(new { message = "Device registered successfully" });
    }
}
