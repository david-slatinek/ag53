using movies_service.Data;
using movies_service.Interfaces;
using movies_service.Models.Database;
using movies_service.Models.Requests;
using movies_service.Models.Responses;

namespace movies_service.Repositories;

/// <summary>
/// Movies repository.
/// </summary>
/// <param name="context">Database context.</param>
public class MoviesRepository(DataContext context) : IMoviesRepository
{
    /// <summary>
    /// Database context.
    /// </summary>
    private DataContext Context { get; } = context;

    /// <inheritdoc />
    public MovieDto CreateMovie(CreateMovie createMovie)
    {
        var releaseDate = DateOnly.Parse(createMovie.Release);
        if (releaseDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BadHttpRequestException("Release date cannot be in the future.");
        }

        var existingMovie =
            Context.Movies.Any(m => m.Title == createMovie.Title && m.Release == releaseDate);

        if (existingMovie)
        {
            throw new BadHttpRequestException("Movie already exists.");
        }

        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = createMovie.Title,
            Description = createMovie.Description,
            Release = DateOnly.Parse(createMovie.Release)
        };

        Context.Movies.Add(movie);
        Context.SaveChanges();

        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Release = movie.Release.ToString("yyyy-MM-dd")
        };
    }

    /// <inheritdoc />
    public MovieDto GetMovie(Guid id)
    {
        var movie = Context.Movies.FirstOrDefault(m => m.Id == id) ??
                    throw new BadHttpRequestException("Movie not found.");

        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Release = movie.Release.ToString("yyyy-MM-dd")
        };
    }
}