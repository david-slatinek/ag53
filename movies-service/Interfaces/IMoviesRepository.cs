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
}