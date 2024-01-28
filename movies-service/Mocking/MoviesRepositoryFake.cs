using movies_service.Interfaces;
using movies_service.Models.Database;
using movies_service.Models.Requests;
using movies_service.Models.Responses;

namespace movies_service.Mocking;

/// <summary>
/// Repository used for unit testing.
/// </summary>
public class MoviesRepositoryFake : IMoviesRepository
{
    private List<Movie> _movies = [];

    /// <inheritdoc />
    public MovieDto CreateMovie(CreateMovie createMovie)
    {
        var releaseDate = DateOnly.Parse(createMovie.Release);
        if (releaseDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BadHttpRequestException("Release date cannot be in the future.");
        }

        var movieExists = _movies.Any(m => m.Title == createMovie.Title && m.Release == releaseDate);
        if (movieExists)
        {
            throw new BadHttpRequestException("Movie already exists.");
        }

        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = createMovie.Title,
            Description = createMovie.Description,
            Release = releaseDate
        };

        _movies.Add(movie);

        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Release = movie.Release.ToString("yyyy-MM-dd")
        };
    }
}