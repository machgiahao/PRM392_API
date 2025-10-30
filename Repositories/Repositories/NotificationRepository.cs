using Repositories.Entities;
using Repositories.Repositories.Interfaces;

namespace Repositories.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly SalesAppDbContext _context;

    public NotificationRepository(SalesAppDbContext context)
    {
        _context = context;
    }

    public void Add(Notification notification)
    {
        _context.Notifications.Add(notification);
    }
}
