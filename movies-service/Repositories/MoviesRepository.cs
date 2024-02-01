using AutoMapper;
using movies_service.Data;
using movies_service.Interfaces;
using movies_service.Models.Database;
using movies_service.Models.Filters;
using movies_service.Models.Requests;
using movies_service.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace movies_service.Repositories;

/// <summary>
/// Movies repository.
/// </summary>
/// <param name="context">Database context.</param>
/// <param name="mapper">Mapper.</param>
public class MoviesRepository(DataContext context, IMapper mapper) : IMoviesRepository
{
    /// <summary>
    /// Database context.
    /// </summary>
    private DataContext Context { get; } = context;

    /// <summary>
    /// Mapper.
    /// </summary>
    private IMapper Mapper { get; } = mapper;

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

        var movie = Mapper.Map<Movie>(createMovie);
        movie.Id = Guid.NewGuid();

        Context.Movies.Add(movie);
        Context.SaveChanges();

        return Mapper.Map<MovieDto>(movie);
    }

    /// <inheritdoc />
    public MovieDto GetMovie(Guid id)
    {
        var movie = Context.Movies.Include(movie => movie.Images).FirstOrDefault(m => m.Id == id) ??
                    throw new BadHttpRequestException($"Movie with id = {id} not found.");

        return Mapper.Map<MovieDto>(movie);
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
                            throw new BadHttpRequestException($"Movie with id = {id} not found.");

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

        return Mapper.Map<MovieDto>(movieToUpdate);
    }

    /// <inheritdoc />
    public void DeleteMovie(Guid id)
    {
        var movie = Context.Movies.FirstOrDefault(m => m.Id == id) ??
                    throw new BadHttpRequestException($"Movie with id = {id} not found.");

        Context.Movies.Remove(movie);
        Context.SaveChanges();
    }

    /// <inheritdoc />
    public List<MovieDto> GetMovies()
    {
        return Mapper.Map<List<MovieDto>>(Context.Movies.Include(movie => movie.Images).ToList());
    }

    /// <inheritdoc />
    public PagedMovies GetPagedMovies(PaginationFilter paginationFilter)
    {
        // use automapper to map the movies to MovieDto

        var movies = Context.Movies.Include(movie => movie.Images)
            .Select(m => Mapper.Map<MovieDto>(m))
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
        var movies = Context.Movies.Include(movie => movie.Images)
            .Where(m => EF.Functions.ILike(m.Title, $"%{title}%"))
            .ToList();
        
        return Mapper.Map<List<MovieDto>>(movies);
    }

    /// <inheritdoc />
    public List<MovieDto> GetMoviesByIds(List<Guid> ids)
    {
        var movies = Context.Movies.Include(movie => movie.Images)
            .Where(m => ids.Contains(m.Id))
            .ToList();
        
        return Mapper.Map<List<MovieDto>>(movies);
    }
}