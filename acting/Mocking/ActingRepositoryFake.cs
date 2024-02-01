using acting.Interfaces;
using acting.Models.Database;
using acting.Models.Requests;
using acting.Models.Responses;
using AutoMapper;

namespace acting.Mocking;

/// <summary>
/// Repository used for unit testing.
/// <param name="mapper">Mapper.</param>
/// </summary>
public class ActingRepositoryFake(IMapper mapper) : IActingRepository
{
    private int _id = 1;
    private List<Acting> _actings = [];
    private IMapper Mapper { get; } = mapper;

    /// <inheritdoc />
    public ActingDto CreateActing(CreateActing createActing)
    {
        var acting = Mapper.Map<Acting>(createActing);
        acting.Id = _id++;

        _actings.Add(acting);

        return Mapper.Map<ActingDto>(acting);
    }

    /// <inheritdoc />
    public bool AlreadyExists(CreateActing createActing)
    {
        return _actings.Any(a => a.ActorId == createActing.ActorId && a.MovieId == createActing.MovieId);
    }

    /// <inheritdoc />
    public void DeleteActing(int id)
    {
        var acting = _actings.Find(a => a.Id == id) ??
                     throw new BadHttpRequestException($"Acting with id = {id} does not exist.");

        _actings.Remove(acting);
    }

    /// <inheritdoc />
    public List<MovieDto> GetMoviesForActor(Guid actorId)
    {
        return _actings.Where(a => a.ActorId == actorId).Select(a => new MovieDto
        {
            Id = a.MovieId
        }).ToList();
    }

    /// <inheritdoc />
    public List<ActingDto> GetActings()
    {
        return _actings.Select(a => Mapper.Map<ActingDto>(a)).ToList();
    }
}