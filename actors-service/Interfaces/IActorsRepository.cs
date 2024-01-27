using actors_service.Models.Database;
using actors_service.Models.Requests;

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
    Actor CreateActor(CreateActor actor);
}