namespace Repositories.Entities;

public class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string Role { get; set; } = null!;

    public ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
