using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<IReadOnlyList<Review>> GetByProductIdAsync(int productId);
    Task<Review> AddAsync(Review review);
}
