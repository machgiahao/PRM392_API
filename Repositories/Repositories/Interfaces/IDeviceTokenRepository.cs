using Repositories.Entities;

namespace Repositories.Repositories.Interfaces;

public interface IDeviceTokenRepository
{
    Task<DeviceToken> GetByTokenAsync(string token);
    Task<List<DeviceToken>> GetTokensByUserIdAsync(int userId);
    void Add(DeviceToken deviceToken);
    void Update(DeviceToken deviceToken);
}
