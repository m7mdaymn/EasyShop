using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs.Reviews;

public class CreateReviewDto
{
    [Required, MaxLength(150)]
    public string ReviewerName { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }
}
