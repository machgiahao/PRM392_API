namespace Repositories.Entities;

public class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? BriefDescription { get; set; }

    public string? FullDescription { get; set; }

    public string? TechnicalSpecifications { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public int? CategoryId { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public Category? Category { get; set; }
}
