namespace Shop.Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public int Stock { get; set; }

    public string? Brand { get; set; }
    public string? Thumbnail { get; set; }

    public bool IsFeatured { get; set; } = false;
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<ProductImage> Images { get; set; }
        = new List<ProductImage>();

    public ICollection<ProductTag> Tags { get; set; }
        = new List<ProductTag>();

    public ICollection<Review> Reviews { get; set; }
        = new List<Review>();
}