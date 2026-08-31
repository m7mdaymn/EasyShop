using Shop.Application.DTOs.Reviews;

namespace Shop.Application.DTOs.Products;

public class ProductDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Brand { get; set; }
    public string? Thumbnail { get; set; }
    public bool IsFeatured { get; set; }
    public double Rating { get; set; }
    public int ReviewsCount { get; set; }
    public int CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
    public IReadOnlyList<string> Images { get; set; } = Array.Empty<string>();
    public IReadOnlyList<string> Tags { get; set; } = Array.Empty<string>();
    public IReadOnlyList<ReviewDto> Reviews { get; set; } = Array.Empty<ReviewDto>();
}
