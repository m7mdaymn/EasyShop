using Shop.Application.DTOs.Common;
using Shop.Application.DTOs.Products;

namespace Shop.Application.Interfaces;

public interface IProductService
{
    Task<PagedResultDto<ProductDto>> GetAllAsync(
        string? search, string? category, int pageNumber, int pageSize);

    Task<ProductDetailsDto?> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductDto>> GetTopAsync(int count);
}
