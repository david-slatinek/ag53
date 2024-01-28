using System.ComponentModel.DataAnnotations;

namespace movies_service.Models.Requests;

/// <summary>
/// Model for creating a movie.
/// </summary>
public class CreateMovie
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(255, ErrorMessage = "Title cannot be longer than 255 characters.")]
    [MinLength(1, ErrorMessage = "Title cannot be empty.")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required.")]
    [MaxLength(255, ErrorMessage = "Description cannot be longer than 255 characters.")]
    [MinLength(1, ErrorMessage = "Description cannot be empty.")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Release date is required.")]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Release date must be in the format YYYY-MM-DD.")]
    public string Release { get; set; } = string.Empty;
}