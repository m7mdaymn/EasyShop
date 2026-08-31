namespace Shop.Domain.Entities;

public class Review
{
    public int Id { get; set; }

    public string ReviewerName { get; set; } = string.Empty;

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}