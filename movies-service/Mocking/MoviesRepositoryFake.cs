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
    private IMoviesRepository _moviesRepositoryImplementation;

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

    /// <inheritdoc />
    public MovieDto GetMovie(Guid id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id) ?? throw new BadHttpRequestException("Movie not found.");

        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Release = movie.Release.ToString("yyyy-MM-dd")
        };
    }

    /// <inheritdoc />
    public MovieDto UpdateMovie(Guid id, UpdateMovie updateMovie)
    {
        var release = DateOnly.Parse(updateMovie.Release);
        if (release > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BadHttpRequestException("Release date cannot be in the future.");
        }

        var movieToUpdate = _movies.FirstOrDefault(m => m.Id == id) ??
                            throw new BadHttpRequestException("Movie not found.");

        movieToUpdate.Title = updateMovie.Title;
        movieToUpdate.Description = updateMovie.Description;
        movieToUpdate.Release = release;

        _movies = _movies.Select(m => m.Id == id ? movieToUpdate : m).ToList();

        return new MovieDto
        {
            Id = movieToUpdate.Id,
            Title = movieToUpdate.Title,
            Description = movieToUpdate.Description,
            Release = movieToUpdate.Release.ToString("yyyy-MM-dd")
        };
    }

    /// <inheritdoc />
    public void DeleteMovie(Guid id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id) ?? throw new BadHttpRequestException("Movie not found.");
        _movies.Remove(movie);
    }
}