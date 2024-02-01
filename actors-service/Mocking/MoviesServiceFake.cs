using actors_service.Interfaces;
using actors_service.Models.Responses;

namespace actors_service.Mocking;

/// <summary>
/// Service used for unit testing.
/// </summary>
public class MoviesServiceFake : IMoviesService
{
    /// <inheritdoc />
    public List<MovieDto> GetMoviesForActor(Guid actorId)
    {
        return [];
    }
}