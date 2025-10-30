
namespace Repositories.Repositories.Interfaces;

public interface INotificationService
{
    Task SendCartUpdatePush(int userId, int totalItems);
}
