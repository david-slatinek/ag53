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

/// <summary>
/// Model for returning image stream.
/// </summary>
public class ImageStream
{
    /// <summary>
    /// Image filename.
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// Content type.
    /// </summary>
    public string ContentType { get; set; } = null!;

    /// <summary>
    /// Image stream.
    /// </summary>
    public Stream Stream { get; set; } = null!;
}