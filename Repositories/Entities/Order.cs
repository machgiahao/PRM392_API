namespace Repositories.Entities;

public class Order
{
    public int OrderId { get; set; }

    public int? CartId { get; set; }

    public int? UserId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string BillingAddress { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public Cart? Cart { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public User? User { get; set; }
}
