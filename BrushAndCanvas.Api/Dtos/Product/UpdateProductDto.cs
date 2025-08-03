namespace BrushAndCanvas.Api.Dtos.Product;

using System.ComponentModel.DataAnnotations;

public class UpdateProductDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Name cannot be over 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500, ErrorMessage = "Description cannot be over 500 characters")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, 1000000)]
    public decimal Price { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    [Range(0, 10000)]
    public int StockQuantity { get; set; }
}
