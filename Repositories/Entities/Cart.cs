namespace Repositories.Entities;

public class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public User? User { get; set; }
}
