using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.Newsletter;
using Shop.Application.Interfaces;

namespace Shop.API.Controllers;

[ApiController]
[Route("api/newsletter")]
public class NewsletterController : ControllerBase
{
    private readonly INewsletterService _newsletter;

    public NewsletterController(INewsletterService newsletter)
    {
        _newsletter = newsletter;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscribeDto dto)
    {
        try
        {
            var created = await _newsletter.SubscribeAsync(dto.Email);
            return created
                ? Ok(new { message = "Subscribed successfully." })
                : Ok(new { message = "Email is already subscribed." });
        }
        catch (FormatException)
        {
            return BadRequest(new { message = "Invalid email address." });
        }
    }
}
