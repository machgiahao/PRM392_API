namespace Repositories.Entities;

public class DeviceToken
{
    public int DeviceTokenId { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime LastUsed { get; set; }

    public virtual User User { get; set; }
}