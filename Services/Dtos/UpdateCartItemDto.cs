using System.ComponentModel.DataAnnotations;

namespace Services.Dtos;

public class UpdateCartItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; }
}
