namespace Services.Dtos;

public class ProductDto
{
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public string BriefDescription { get; set; }
    public decimal Price { get; set; }
    public string ImageURL { get; set; }
    public string CategoryName { get; set; }
    public string Brand { get; set; }
    public double Rating { get; set; }
}
