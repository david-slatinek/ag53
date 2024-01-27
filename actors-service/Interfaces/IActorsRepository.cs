using actors_service.Models.Requests;
using actors_service.Models.Responses;

namespace actors_service.Interfaces;

/// <summary>
/// Interface for the actors repository.
/// </summary>
public interface IActorsRepository
{
    /// <summary>
    /// Adds an actor to the database.
    /// </summary>
    /// <param name="actor">Actor to add.</param>
    /// <returns>Added actor.</returns>
    ActorDto CreateActor(CreateActor actor);

    /// <summary>
    /// Get actor by id.
    /// </summary>
    /// <param name="id">Actor id.</param>
    /// <returns>Actor.</returns>
    ActorDto GetActorById(Guid id);

    /// <summary>
    /// Update actor.
    /// </summary>
    /// <param name="id">Actor id.</param>
    /// <param name="actor">Actor data.</param>
    /// <returns>Updated actor.</returns>
    ActorDto UpdateActor(Guid id, UpdateActor actor);
}