

namespace Services.Dtos;

public class CartDto
{
    public int CartId { get; set; }
    public decimal TotalCartPrice { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}
