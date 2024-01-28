using System.ComponentModel.DataAnnotations;

namespace acting.Models.Requests;

/// <summary>
/// Model for creating an acting, i.e. adding an actor to a movie.
/// </summary>
public class CreateActing
{
    /// <summary>
    /// Movie id.
    /// </summary>
    [Required(ErrorMessage = "Actor id is required.")]
    public Guid ActorId { get; set; }

    /// <summary>
    /// Movie id.
    /// </summary>
    [Required(ErrorMessage = "Movie id is required.")]
    public Guid MovieId { get; set; }
}