using movies_service.Models.Filters;
using movies_service.Models.Requests;
using movies_service.Models.Responses;

namespace movies_service.Interfaces;

/// <summary>
/// Interface for the movies repository.
/// </summary>
public interface IMoviesRepository
{
    /// <summary>
    /// Adds a movie to the database.
    /// </summary>
    /// <param name="createMovie">Movie to add.</param>
    /// <returns>Added movie.</returns>
    MovieDto CreateMovie(CreateMovie createMovie);

    /// <summary>
    /// Get a movie by id.
    /// </summary>
    /// <param name="id">Movie id.</param>
    /// <returns>Movie.</returns>
    MovieDto GetMovie(Guid id);

    /// <summary>
    /// Update a movie.
    /// </summary>
    /// <param name="id">Movie id.</param>
    /// <param name="updateMovie">Movie data.</param>
    /// <returns>Updated movie.</returns>
    MovieDto UpdateMovie(Guid id, UpdateMovie updateMovie);

    /// <summary>
    /// Delete a movie.
    /// </summary>
    /// <param name="id">Movie id.</param>
    void DeleteMovie(Guid id);

    /// <summary>
    /// Get all movies.
    /// </summary>
    List<MovieDto> GetMovies();

    /// <summary>
    /// Get movies with pagination.
    /// </summary>
    /// <param name="paginationFilter">Pagination filter.</param>
    /// <returns>Paged movies.</returns>
    PagedMovies GetPagedMovies(PaginationFilter paginationFilter);
}