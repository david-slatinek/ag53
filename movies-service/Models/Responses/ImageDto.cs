namespace movies_service.Models.Responses;

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