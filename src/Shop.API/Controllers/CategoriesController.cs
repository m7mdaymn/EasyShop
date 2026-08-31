using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;

namespace Shop.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categories;

    public CategoriesController(ICategoryService categories)
    {
        _categories = categories;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _categories.GetAllAsync());
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTop([FromQuery] int count = 6)
    {
        return Ok(await _categories.GetTopAsync(count));
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var category = await _categories.GetBySlugAsync(slug);
        return category is null ? NotFound() : Ok(category);
    }

    [HttpGet("{slug}/products")]
    public async Task<IActionResult> GetProducts(
        string slug,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 12)
    {
        var result = await _categories.GetProductsAsync(slug, pageNumber, pageSize);
        return result is null ? NotFound() : Ok(result);
    }
}
