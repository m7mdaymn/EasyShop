using Shop.Application.DTOs.Reviews;
using Shop.Application.Interfaces;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;

namespace Shop.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviews;
    private readonly IProductRepository _products;

    public ReviewService(IReviewRepository reviews, IProductRepository products)
    {
        _reviews = reviews;
        _products = products;
    }

    public async Task<IReadOnlyList<ReviewDto>> GetByProductIdAsync(int productId)
    {
        var reviews = await _reviews.GetByProductIdAsync(productId);
        return reviews.Select(Map).ToList();
    }

    public async Task<ReviewDto?> AddAsync(int productId, CreateReviewDto dto)
    {
        if (!await _products.ExistsAsync(productId))
            return null;

        var review = new Review
        {
            ProductId = productId,
            ReviewerName = dto.ReviewerName.Trim(),
            Rating = dto.Rating,
            Comment = string.IsNullOrWhiteSpace(dto.Comment) ? null : dto.Comment.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        review = await _reviews.AddAsync(review);
        return Map(review);
    }

    private static ReviewDto Map(Review review) => new()
    {
        Id = review.Id,
        ReviewerName = review.ReviewerName,
        Rating = review.Rating,
        Comment = review.Comment,
        CreatedAt = review.CreatedAt
    };
}
