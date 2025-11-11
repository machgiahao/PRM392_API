using System.Text.Json.Serialization;

namespace Repositories.Entities;

public class CartItem
{
    public int CartItemId { get; set; }

    public int? CartId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    [JsonIgnore]
    public Cart? Cart { get; set; }

    public Product? Product { get; set; }
}
