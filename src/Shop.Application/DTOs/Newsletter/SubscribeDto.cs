using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs.Newsletter;

public class SubscribeDto
{
    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;
}
