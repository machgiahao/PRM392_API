namespace Services.Interfaces;

public interface IDeviceTokenService
{
    Task RegisterDeviceAsync(int userId, string token);
}
