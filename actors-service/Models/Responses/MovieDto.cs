namespace actors_service.Models.Responses;

/// <summary>
/// Movie response model.
/// </summary>
public class MovieDto
{
    /// <summary>
    /// Movie's unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Movie title.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Movie description.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Release date of the movie.
    /// </summary>
    public string Release { get; set; } = null!;

    /// <summary>
    /// Images for the movie.
    /// </summary>
    public List<ImageDto> Images { get; set; } = [];
}

/// <summary>
/// Image DTO.
/// </summary>
public class ImageDto
{
    /// <summary>
    /// Image ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Image filename.
    /// </summary>
    public string FileName { get; set; } = null!;
}