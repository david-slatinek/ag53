using movies_service.Models.Database;
using movies_service.Models.Responses;

namespace movies_service.Interfaces;

/// <summary>
/// Interface for the images repository.
/// </summary>
public interface IImagesRepository
{
    /// <summary>
    /// Add image data to the database.
    /// </summary>
    /// <param name="image">Image data.</param>
    /// <returns>Added image.</returns>
    public Image CreateImage(Image image);

    /// <summary>
    /// Get image data from the database.
    /// </summary>
    /// <param name="imageId">Image ID.</param>
    /// <returns>Image data.</returns>
    public Image GetImage(int imageId);
}