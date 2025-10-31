using Repositories.Entities;
using System.Security.Cryptography;

namespace Repositories.Repositories.Interfaces;

public interface INotificationRepository
{
    void Add(Notification notification);
}
