using Shop.Application.DTOs.Reviews;

namespace Shop.Application.Interfaces;

public interface IReviewService
{
    Task<IReadOnlyList<ReviewDto>> GetByProductIdAsync(int productId);
    Task<ReviewDto?> AddAsync(int productId, CreateReviewDto dto);
}
