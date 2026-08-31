using Shop.Application.DTOs.Common;
using Shop.Application.DTOs.Products;
using Shop.Application.DTOs.Reviews;
using Shop.Application.Interfaces;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;

namespace Shop.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _products;

    public ProductService(IProductRepository products)
    {
        _products = products;
    }

    public async Task<PagedResultDto<ProductDto>> GetAllAsync(
        string? search, string? category, int pageNumber, int pageSize)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var (items, totalCount) = await _products.GetPagedAsync(
            search?.Trim(), category?.Trim(), pageNumber, pageSize);

        return new PagedResultDto<ProductDto>
        {
            Items = items.Select(MapList).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ProductDetailsDto?> GetByIdAsync(int id)
    {
        var product = await _products.GetByIdAsync(id);
        if (product is null || !product.IsActive)
            return null;

        return new ProductDetailsDto
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Brand = product.Brand,
            Thumbnail = product.Thumbnail,
            IsFeatured = product.IsFeatured,
            Rating = GetRating(product),
            ReviewsCount = product.Reviews.Count,
            CategoryId = product.CategoryId,
            Category = product.Category.Name,
            CategorySlug = product.Category.Slug,
            Images = product.Images.OrderBy(x => x.Order).Select(x => x.ImageUrl).ToList(),
            Tags = product.Tags.Select(x => x.Name).OrderBy(x => x).ToList(),
            Reviews = product.Reviews
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ReviewDto
                {
                    Id = x.Id,
                    ReviewerName = x.ReviewerName,
                    Rating = x.Rating,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt
                }).ToList()
        };
    }

    public async Task<IReadOnlyList<ProductDto>> GetTopAsync(int count)
    {
        count = Math.Clamp(count, 1, 20);
        var products = await _products.GetFeaturedAsync(count);
        return products.Select(MapList).ToList();
    }

    private static ProductDto MapList(Product product) => new()
    {
        Id = product.Id,
        Title = product.Title,
        Price = product.Price,
        Stock = product.Stock,
        Brand = product.Brand,
        Thumbnail = product.Thumbnail,
        Rating = GetRating(product),
        ReviewsCount = product.Reviews.Count,
        Category = product.Category.Name,
        CategorySlug = product.Category.Slug
    };

    private static double GetRating(Product product) =>
        product.Reviews.Count == 0
            ? 0
            : Math.Round(product.Reviews.Average(x => x.Rating), 2);
}
