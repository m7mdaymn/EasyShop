using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.Reviews;
using Shop.Application.Interfaces;

namespace Shop.API.Controllers;

[ApiController]
[Route("api/products/{productId:int}/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviews;

    public ReviewsController(IReviewService reviews)
    {
        _reviews = reviews;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int productId)
    {
        return Ok(await _reviews.GetByProductIdAsync(productId));
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, [FromBody] CreateReviewDto dto)
    {
        var review = await _reviews.AddAsync(productId, dto);
        if (review is null)
            return NotFound(new { message = "Product not found." });

        return Created($"/api/products/{productId}/reviews", review);
    }
}
