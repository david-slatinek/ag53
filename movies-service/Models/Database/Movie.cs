using System.ComponentModel.DataAnnotations.Schema;

namespace movies_service.Models.Database;

/// <summary>
/// Movie model for the database.
/// </summary>
[Table("movies")]
public class Movie
{
    /// <summary>
    /// Movie's unique identifier.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Movie title.
    /// </summary>
    [Column("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Movie description.
    /// </summary>
    [Column("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Release date of the movie.
    /// </summary>
    [Column("release")]
    public DateOnly Release { get; set; }

    /// <summary>
    /// Images of the movie.
    /// </summary>
    public List<Image> Images { get; set; } = null!;
}