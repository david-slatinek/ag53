using System.Security.Cryptography;
using Minio;
using Minio.DataModel.Args;
using movies_service.Interfaces;
using movies_service.Models.Database;
using movies_service.Models.Responses;

namespace movies_service.Services;

/// <summary>
/// Service for images.
/// </summary>
/// <param name="moviesRepository">Movies repository.</param>
/// <param name="minioClient">Minio client.</param>
/// <param name="imagesRepository">Images repository.</param>
public class ImagesService(
    IMoviesRepository moviesRepository,
    IMinioClient minioClient,
    IImagesRepository imagesRepository) : IImagesService
{
    /// <summary>
    /// Movies repository.
    /// </summary>
    private IMoviesRepository MoviesRepository { get; } = moviesRepository;

    /// <summary>
    /// Minio client.
    /// </summary>
    private IMinioClient MinioClient { get; } = minioClient;

    /// <summary>
    /// Images repository.
    /// </summary>
    private IImagesRepository ImagesRepository { get; } = imagesRepository;

    /// <summary>
    /// Hashing algorithm.
    /// </summary>
    private SHA256 Sha256 { get; } = SHA256.Create();

    /// <inheritdoc />
    public async Task<List<ImageDto>> AddImages(Guid movieId, List<IFormFile> images)
    {
        var movie = MoviesRepository.GetMovie(movieId);

        CreateBucketIfNotExists(movie.Id.ToString());

        List<ImageDto> addedImages = [];

        foreach (var image in images)
        {
            var extension = image.FileName.Split(".").Last();

            var hash = await Sha256.ComputeHashAsync(image.OpenReadStream());
            var hashString = Convert.ToBase64String(hash);

            if (movie.Images.Any(x => x.FileName == $"{hashString}.{extension}"))
            {
                throw new BadHttpRequestException("Image already exists.");
            }

            hashString = hashString.Replace("/", "_");

            var createdImage = new Image
            {
                MovieId = movieId,
                FileName = $"{hashString}.{extension}"
            };
            createdImage = ImagesRepository.CreateImage(createdImage);

            addedImages.Add(new ImageDto
            {
                Id = createdImage.Id,
                FileName = createdImage.FileName
            });

            await using var stream = image.OpenReadStream();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(movie.Id.ToString())
                .WithObject(createdImage.FileName)
                .WithStreamData(stream)
                .WithContentType(image.ContentType)
                .WithObjectSize(stream.Length);

            await MinioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }

        return addedImages;
    }

    /// <inheritdoc />
    public async Task<ImageStream> GetImage(int imageId)
    {
        var image = ImagesRepository.GetImage(imageId);

        var memoryStream = new MemoryStream();

        var args = new GetObjectArgs()
            .WithBucket(image.MovieId.ToString())
            .WithObject(image.FileName)
            .WithCallbackStream(x =>
            {
                x.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
            });

        await MinioClient.GetObjectAsync(args).ConfigureAwait(false);

        var contentType = image.FileName.Split(".").Last() switch
        {
            "png" => "image/png",
            "jpg" => "image/jpeg",
            _ => "binary/octet-stream"
        };

        return new ImageStream
        {
            FileName = image.FileName,
            ContentType = contentType,
            Stream = memoryStream
        };
    }

    /// <inheritdoc />
    public async Task DeleteImage(int imageId)
    {
        var image = ImagesRepository.GetImage(imageId);

        ImagesRepository.DeleteImage(imageId);

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(image.MovieId.ToString())
            .WithObject(image.FileName);

        await MinioClient.RemoveObjectAsync(removeObjectArgs).ConfigureAwait(false);
    }

    /// <summary>
    /// Create bucket if it does not exist.
    /// </summary>
    /// <param name="bucketName">Bucket name.</param>
    private async void CreateBucketIfNotExists(string bucketName)
    {
        var beArgs = new BucketExistsArgs().WithBucket(bucketName);
        var found = await MinioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);

        if (!found)
        {
            var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
            await MinioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
        }
    }
}