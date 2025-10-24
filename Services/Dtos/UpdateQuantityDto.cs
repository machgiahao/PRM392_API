using System.ComponentModel.DataAnnotations;

namespace Services.Dtos;

public class UpdateQuantityDto
{
    [Range(1, 100)]
    public int Quantity { get; set; }
}
