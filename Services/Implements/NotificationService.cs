using FirebaseAdmin.Messaging;
using Repositories.Repositories.Interfaces;
using Repositories.Uow;

namespace Repositories.Repositories;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepo;
    private readonly IDeviceTokenRepository _deviceTokenRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly FirebaseMessaging _firebaseMessaging;

    public NotificationService(
        INotificationRepository notificationRepo,
        IDeviceTokenRepository deviceTokenRepo,
        IUnitOfWork unitOfWork)
    {
        _notificationRepo = notificationRepo;
        _deviceTokenRepo = deviceTokenRepo;
        _unitOfWork = unitOfWork;
        _firebaseMessaging = FirebaseMessaging.DefaultInstance;
    }

    public async Task SendCartUpdatePush(int userId, int totalItems)
    {
        _notificationRepo.Add(new Entities.Notification
        {
            UserId = userId,
            Message = $"Your cart has been updated. Total items: {totalItems}",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });

        var userTokens = (await _deviceTokenRepo.GetTokensByUserIdAsync(userId))
                            .Select(t => t.Token).ToList();

        if (!userTokens.Any())
        {
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        var dataPayload = new Dictionary<string, string>
            {
                { "type", "cart_update" },
                { "totalItems", totalItems.ToString() }
            };

        var message = new MulticastMessage()
        {
            Tokens = userTokens,
            Data = dataPayload 
        };

        _ = _firebaseMessaging.SendEachForMulticastAsync(message);

        await _unitOfWork.SaveChangesAsync();
    }
}
