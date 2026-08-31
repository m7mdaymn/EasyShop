using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _db;

    public CategoryRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync()
    {
        return await _db.Categories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Products)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Category>> GetTopAsync(int count)
    {
        return await _db.Categories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Products)
            .OrderByDescending(x => x.Products.Count(p => p.IsActive))
            .ThenBy(x => x.Name)
            .Take(count)
            .ToListAsync();
    }

    public Task<Category?> GetBySlugAsync(string slug) =>
        _db.Categories
            .AsNoTracking()
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.IsActive && x.Slug == slug);
}
