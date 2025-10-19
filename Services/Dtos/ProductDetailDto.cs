namespace Services.Dtos
{
    public class ProductDetailDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? BriefDescription { get; set; }

        public string? FullDescription { get; set; }

        public string? TechnicalSpecifications { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? Brand { get; set; }

        public double Rating { get; set; }

        public int SoldCount { get; set; }

        public string? CategoryName { get; set; }
    }
}
