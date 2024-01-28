using acting.Models.Database;
using acting.Models.Requests;

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
}