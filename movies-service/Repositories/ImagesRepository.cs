using movies_service.Data;
using movies_service.Interfaces;
using movies_service.Models.Database;

namespace movies_service.Repositories;

/// <summary>
/// Images repository.
/// </summary>
/// <param name="context">Database context.</param>
public class ImagesRepository(DataContext context) : IImagesRepository
{
    /// <summary>
    /// Database context.
    /// </summary>
    private DataContext Context { get; } = context;

    /// <inheritdoc />
    public Image CreateImage(Image image)
    {
        Context.Images.Add(image);
        Context.SaveChanges();
        return image;
    }

    /// <inheritdoc />
    public Image GetImage(int imageId)
    {
        var image = Context.Images.Find(imageId) ??
                    throw new BadHttpRequestException($"Image with id = {imageId} does not exist.");
        return image;
    }

    /// <inheritdoc />
    public void DeleteImage(int id)
    {
        var image = Context.Images.Find(id) ??
                    throw new BadHttpRequestException($"Image with id = {id} does not exist.");
        Context.Images.Remove(image);
        Context.SaveChanges();
    }
}