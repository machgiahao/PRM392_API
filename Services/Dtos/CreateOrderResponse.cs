using Repositories.Entities;

namespace Services.Dtos
{
    public class CreateOrderResponse
    {
        public int OrderId { get; set; }

        public int? CartId { get; set; }

        public int? UserId { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string BillingAddress { get; set; } = null!;

        public string OrderStatus { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<ProductOrderItemDto> Products { get; set; } = new List<ProductOrderItemDto>();
    }

    public class ProductOrderItemDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
