namespace Repositories.Entities;

public class Payment
{
    public int PaymentId { get; set; }

    public int? OrderId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public Order? Order { get; set; }
}
