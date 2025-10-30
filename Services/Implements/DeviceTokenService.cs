using Repositories.Entities;
using Repositories.Repositories.Interfaces;
using Repositories.Uow;
using Services.Interfaces;

namespace Services.Implements;

public class DeviceTokenService : IDeviceTokenService
{
    private readonly IDeviceTokenRepository _deviceTokenRepo;
    private readonly IUnitOfWork _unitOfWork;

    public DeviceTokenService(IDeviceTokenRepository deviceTokenRepo, IUnitOfWork unitOfWork)
    {
        _deviceTokenRepo = deviceTokenRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task RegisterDeviceAsync(int userId, string token)
    {
        var existingToken = await _deviceTokenRepo.GetByTokenAsync(token);
        if (existingToken == null)
        {
            _deviceTokenRepo.Add(new DeviceToken
            {
                UserId = userId,
                Token = token,
                LastUsed = DateTime.UtcNow
            });
        }
        else
        {
            existingToken.UserId = userId;
            existingToken.LastUsed = DateTime.UtcNow;
            _deviceTokenRepo.Update(existingToken);
        }
        await _unitOfWork.SaveChangesAsync();
    }
}
