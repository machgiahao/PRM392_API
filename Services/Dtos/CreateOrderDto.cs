namespace Services.Dtos
{
    public class CreateOrderDto
    {
        public string PaymentMethod { get; set; } = null!;
        public string BillingAddress { get; set; } = null!;
    }
}
