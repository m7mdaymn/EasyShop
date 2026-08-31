namespace Shop.Domain.Entities;

public class ProductTag
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}