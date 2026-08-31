using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        string? search, string? categorySlug, int pageNumber, int pageSize)
    {
        var query = _db.Products
            .AsNoTracking()
            .Where(x => x.IsActive && x.Category.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(x =>
                x.Title.Contains(term) ||
                (x.Brand != null && x.Brand.Contains(term)) ||
                x.Description.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            var slug = categorySlug.Trim();
            query = query.Where(x => x.Category.Slug == slug);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Include(x => x.Category)
            .Include(x => x.Reviews)
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public Task<Product?> GetByIdAsync(int id) =>
        _db.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Images)
            .Include(x => x.Tags)
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

    public async Task<IReadOnlyList<Product>> GetFeaturedAsync(int count)
    {
        return await _db.Products
            .AsNoTracking()
            .Where(x => x.IsActive && x.Category.IsActive)
            .Include(x => x.Category)
            .Include(x => x.Reviews)
            .OrderByDescending(x => x.IsFeatured)
            .ThenByDescending(x => x.Reviews.Count)
            .ThenByDescending(x => x.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public Task<bool> ExistsAsync(int id) =>
        _db.Products.AnyAsync(x => x.Id == id && x.IsActive);
}
