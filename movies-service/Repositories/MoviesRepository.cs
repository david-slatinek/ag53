using movies_service.Data;
using movies_service.Interfaces;
using movies_service.Models.Database;
using movies_service.Models.Filters;
using movies_service.Models.Requests;
using movies_service.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

    /// <inheritdoc />
    public MovieDto UpdateMovie(Guid id, UpdateMovie updateMovie)
    {
        var release = DateOnly.Parse(updateMovie.Release);
        if (release > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BadHttpRequestException("Release date cannot be in the future.");
        }

        var movieToUpdate = Context.Movies.FirstOrDefault(m => m.Id == id) ??
                            throw new BadHttpRequestException("Movie not found.");

        var movieExists = Context.Movies.Any(m =>
            m.Title == updateMovie.Title && m.Release == release && m.Release == release && m.Id != id);

        if (movieExists)
        {
            throw new BadHttpRequestException("Movie already exists.");
        }

        movieToUpdate.Title = updateMovie.Title;
        movieToUpdate.Description = updateMovie.Description;
        movieToUpdate.Release = release;

        Context.SaveChanges();

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
        var movie = Context.Movies.FirstOrDefault(m => m.Id == id) ??
                    throw new BadHttpRequestException("Movie not found.");

        Context.Movies.Remove(movie);
        Context.SaveChanges();
    }

    /// <inheritdoc />
    public List<MovieDto> GetMovies()
    {
        return Context.Movies.Select(m => new MovieDto
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
        var movies = Context.Movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Release = m.Release.ToString("yyyy-MM-dd")
            })
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToList();

        var totalRecords = Context.Movies.Count();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)paginationFilter.PageSize);

        if (paginationFilter.PageNumber > totalPages && totalPages > 0)
        {
            throw new BadHttpRequestException("Page number is greater than total pages.");
        }

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
        var movies = Context.Movies
            .Where(m => EF.Functions.ILike(m.Title, $"%{title}%"))
            .ToList();

        return movies.Select(m => new MovieDto
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            Release = m.Release.ToString("yyyy-MM-dd")
        }).ToList();
    }
}