using acting.Models.Database;
using acting.Models.Requests;
using acting.Models.Responses;

namespace acting.Interfaces;

/// <summary>
/// Acting service.
/// </summary>
public interface IActingService
{
    /// <summary>
    /// Create an acting, i.e. add an actor to a movie.
    /// </summary>
    /// <param name="createActing">Acting data.</param>
    /// <returns>Acting.</returns>
    ActingDto CreateActing(CreateActing createActing);
}