namespace Shop.Application.DTOs.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Brand { get; set; }
    public string? Thumbnail { get; set; }
    public double Rating { get; set; }
    public int ReviewsCount { get; set; }
    public string Category { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
}
