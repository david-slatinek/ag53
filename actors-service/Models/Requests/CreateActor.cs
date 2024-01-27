using System.ComponentModel.DataAnnotations;

namespace actors_service.Models.Requests;

/// <summary>
/// Model for creating an actor.
/// </summary>
public class CreateActor
{
    /// <summary>
    /// Actor's first name.
    /// </summary>
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(255, ErrorMessage = "First name cannot be longer than 255 characters.")]
    [MinLength(1, ErrorMessage = "First name cannot be empty.")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Actor's last name.
    /// </summary>
    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(255, ErrorMessage = "Last name cannot be longer than 255 characters.")]
    [MinLength(1, ErrorMessage = "Last name cannot be empty.")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Actor's birth date.
    /// </summary>
    [Required(ErrorMessage = "Birth date is required.")]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Birth date must be in the format YYYY-MM-DD.")]
    public string BirthDate { get; set; } = string.Empty;
}