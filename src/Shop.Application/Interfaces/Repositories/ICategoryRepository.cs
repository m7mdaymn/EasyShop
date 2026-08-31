using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync();
    Task<IReadOnlyList<Category>> GetTopAsync(int count);
    Task<Category?> GetBySlugAsync(string slug);
}
