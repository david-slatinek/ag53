using movies_service.Interfaces;
using movies_service.Models.Database;
using movies_service.Models.Filters;
using movies_service.Models.Requests;
using movies_service.Models.Responses;

namespace movies_service.Mocking;

/// <summary>
/// Repository used for unit testing.
/// </summary>
public class MoviesRepositoryFake : IMoviesRepository
{
    /// <summary>
    /// List of movies.
    /// </summary>
    private List<Movie> _movies = [];

    /// <inheritdoc />
    public MovieDto CreateMovie(CreateMovie createMovie)
    {
        var releaseDate = DateOnly.Parse(createMovie.Release);
        if (releaseDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BadHttpRequestException("Release date cannot be in the future.");
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
        var movie = _movies.FirstOrDefault(m => m.Id == id) ??
                    throw new BadHttpRequestException($"Movie with id = {id} not found.");

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
                            throw new BadHttpRequestException($"Movie with id = {id} not found.");

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
        var movie = _movies.FirstOrDefault(m => m.Id == id) ??
                    throw new BadHttpRequestException($"Movie with id = {id} not found.");
        _movies.Remove(movie);
    }

    /// <inheritdoc />
    public List<MovieDto> GetMovies()
    {
        return _movies.Select(m => new MovieDto
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            Release = m.Release.ToString("yyyy-MM-dd")
        }).ToList();
    }

    /// <inheritdoc />
    public PagedMovies GetPagedMovies(PaginationFilter paginationFilter)
    {
        var movies = _movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Release = m.Release.ToString("yyyy-MM-dd")
            })
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToList();

        var totalRecords = _movies.Count;
        var totalPages = (int)Math.Ceiling(totalRecords / (double)paginationFilter.PageSize);

        return new PagedMovies
        {
            Movies = movies,
            PageNumber = paginationFilter.PageNumber,
            PageSize = paginationFilter.PageSize,
            TotalPages = totalPages,
            TotalRecords = totalRecords
        };
    }

    /// <inheritdoc />
    public List<MovieDto> GetMovieByTitle(string title)
    {
        return _movies.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Release = m.Release.ToString("yyyy-MM-dd")
            }).ToList();
    }

    /// <inheritdoc />
    public List<MovieDto> GetMoviesByIds(List<Guid> ids)
    {
        return _movies.Where(m => ids.Contains(m.Id))
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Release = m.Release.ToString("yyyy-MM-dd")
            }).ToList();
    }
}