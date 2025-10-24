

namespace Services.Dtos;

public class CartDto
{
    public int CartId { get; set; }
    public List<CartItemDto> Items { get; set; }
    public decimal TotalCartPrice { get; set; }
}
