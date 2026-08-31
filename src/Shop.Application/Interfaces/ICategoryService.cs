using Shop.Application.DTOs.Categories;
using Shop.Application.DTOs.Common;
using Shop.Application.DTOs.Products;

namespace Shop.Application.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync();
    Task<IReadOnlyList<CategoryDto>> GetTopAsync(int count);
    Task<CategoryDto?> GetBySlugAsync(string slug);
    Task<PagedResultDto<ProductDto>?> GetProductsAsync(string slug, int pageNumber, int pageSize);
}
