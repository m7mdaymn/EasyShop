using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        string? search, string? categorySlug, int pageNumber, int pageSize);

    Task<Product?> GetByIdAsync(int id);
    Task<IReadOnlyList<Product>> GetFeaturedAsync(int count);
    Task<bool> ExistsAsync(int id);
}
