using movies_service.Models.Database;

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

    /// <summary>
    /// Delete image data from the database.
    /// </summary>
    /// <param name="id">Image ID.</param>
    public void DeleteImage(int id);
}