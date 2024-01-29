using System.ComponentModel.DataAnnotations;

namespace movies_service.Models.Requests;

/// <summary>
/// Model for uploading image.
/// </summary>
public class UploadImages
{
    /// <summary>
    /// Image to upload.
    /// </summary>
    [Required(ErrorMessage = "Images are required.")]
    public IFormFileCollection Images { get; set; } = default!;
}