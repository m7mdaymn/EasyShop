using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;

namespace Shop.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _products;

    public ProductsController(IProductService products)
    {
        _products = products;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 12)
    {
        return Ok(await _products.GetAllAsync(search, category, pageNumber, pageSize));
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTop([FromQuery] int count = 3)
    {
        return Ok(await _products.GetTopAsync(count));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _products.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }
}
