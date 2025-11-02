namespace Services.Dtos
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }

        public int? CartId { get; set; }

        public int? UserId { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string BillingAddress { get; set; } = null!;

        public string OrderStatus { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }
        public ICollection<CartItemDto> Products { get; set; } = new List<CartItemDto>();
    }
}
