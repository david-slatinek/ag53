using actors_service.Models.Responses;

namespace actors_service.Interfaces;

/// <summary>
/// Movies service interface.
/// </summary>
public interface IMoviesService
{
    /// <summary>
    /// Get all movies for an actor.
    /// </summary>
    /// <param name="actorId">Actor id.</param>
    /// <returns>List of movies for the actor.</returns>
    List<MovieDto> GetMoviesForActor(Guid actorId);
}