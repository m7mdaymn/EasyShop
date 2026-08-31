using Shop.Application.DTOs.Categories;
using Shop.Application.DTOs.Common;
using Shop.Application.DTOs.Products;
using Shop.Application.Interfaces;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;

namespace Shop.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categories;
    private readonly IProductRepository _products;

    public CategoryService(ICategoryRepository categories, IProductRepository products)
    {
        _categories = categories;
        _products = products;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync()
    {
        var items = await _categories.GetAllAsync();
        return items.Select(MapCategory).ToList();
    }

    public async Task<IReadOnlyList<CategoryDto>> GetTopAsync(int count)
    {
        count = Math.Clamp(count, 1, 20);
        var items = await _categories.GetTopAsync(count);
        return items.Select(MapCategory).ToList();
    }

    public async Task<CategoryDto?> GetBySlugAsync(string slug)
    {
        var category = await _categories.GetBySlugAsync(slug.Trim());
        return category is null ? null : MapCategory(category);
    }

    public async Task<PagedResultDto<ProductDto>?> GetProductsAsync(
        string slug, int pageNumber, int pageSize)
    {
        var category = await _categories.GetBySlugAsync(slug.Trim());
        if (category is null)
            return null;

        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var (items, totalCount) = await _products.GetPagedAsync(
            null, category.Slug, pageNumber, pageSize);

        return new PagedResultDto<ProductDto>
        {
            Items = items.Select(MapProduct).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private static CategoryDto MapCategory(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Slug = category.Slug,
        Image = category.Image,
        ProductsCount = category.Products.Count(x => x.IsActive)
    };

    private static ProductDto MapProduct(Product product) => new()
    {
        Id = product.Id,
        Title = product.Title,
        Price = product.Price,
        Stock = product.Stock,
        Brand = product.Brand,
        Thumbnail = product.Thumbnail,
        Rating = product.Reviews.Count == 0
            ? 0
            : Math.Round(product.Reviews.Average(x => x.Rating), 2),
        ReviewsCount = product.Reviews.Count,
        Category = product.Category.Name,
        CategorySlug = product.Category.Slug
    };
}
