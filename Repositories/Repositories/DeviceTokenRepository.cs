using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Repositories.Interfaces;

namespace Repositories.Repositories;

public class DeviceTokenRepository : IDeviceTokenRepository
{
    private readonly SalesAppDbContext _context;

    public DeviceTokenRepository(SalesAppDbContext context) { _context = context; }

    public async Task<DeviceToken> GetByTokenAsync(string token)
    {
        return await _context.DeviceTokens.FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task<List<DeviceToken>> GetTokensByUserIdAsync(int userId)
    {
        return await _context.DeviceTokens.Where(t => t.UserId == userId).ToListAsync();
    }

    public void Add(DeviceToken deviceToken)
    {
        _context.DeviceTokens.Add(deviceToken);
    }

    public void Update(DeviceToken deviceToken)
    {
        _context.DeviceTokens.Update(deviceToken);
    }
}
