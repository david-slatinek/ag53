using acting.Models.Database;
using acting.Models.Requests;
using acting.Models.Responses;

namespace acting.Interfaces;

/// <summary>
/// Interface for the acting service.
/// </summary>
public interface IActingRepository
{
    /// <summary>
    /// Add an actor to a movie.
    /// </summary>
    /// <param name="createActing">Acting data.</param>
    /// <returns>Created acting.</returns>
    Acting CreateActing(CreateActing createActing);

    /// <summary>
    /// Check if an acting already exists.
    /// </summary>
    /// <param name="createActing">Acting data.</param>
    /// <returns>True if the acting already exists, false otherwise.</returns>
    bool AlreadyExists(CreateActing createActing);

    /// <summary>
    /// Delete an acting.
    /// </summary>
    /// <param name="id">Acting ID.</param>
    void DeleteActing(int id);

    /// <summary>
    /// Get all movies for an actor.
    /// </summary>
    /// <param name="actorId">Actor ID.</param>
    /// <returns>List of movies for the actor.</returns>
    List<MovieDto> GetMoviesForActor(Guid actorId);
}