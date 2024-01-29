using movies_service.Models.Responses;

namespace movies_service.Interfaces;

/// <summary>
/// Interface for images service.
/// </summary>
public interface IImagesService
{
    /// <summary>
    /// Add images for a movie.
    /// </summary>
    /// <param name="movieId">Movie id.</param>
    /// <param name="images">Images.</param>
    /// <returns>A list of added images.</returns>
    Task<List<ImageDto>> AddImages(Guid movieId, List<IFormFile> images);

    /// <summary>
    /// Get image.
    /// </summary>
    /// <param name="imageId">Image id.</param>
    /// <returns>Image stream.</returns>
    Task<ImageStream> GetImage(int imageId);

    /// <summary>
    /// Delete image.
    /// </summary>
    /// <param name="imageId">Image id.</param>
    Task DeleteImage(int imageId);
}