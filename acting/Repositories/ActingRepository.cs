using acting.Data;
using acting.Interfaces;
using acting.Models.Database;
using acting.Models.Requests;
using acting.Models.Responses;
using AutoMapper;

namespace acting.Repositories;

/// <summary>
/// Acting repository.
/// </summary>
/// <param name="context">Database context.</param>
/// <param name="mapper">Mapper.</param>
public class ActingRepository(DataContext context, IMapper mapper) : IActingRepository
{
    /// <summary>
    /// Database context.
    /// </summary>
    private DataContext Context { get; } = context;

    /// <summary>
    /// Mapper.
    /// </summary>
    private IMapper Mapper { get; } = mapper;

    /// <inheritdoc />
    public ActingDto CreateActing(CreateActing createActing)
    {
        var acting = Mapper.Map<Acting>(createActing);

        Context.Acting.Add(acting);
        Context.SaveChanges();

        return Mapper.Map<ActingDto>(acting);
    }

    /// <inheritdoc />
    public bool AlreadyExists(CreateActing createActing)
    {
        return Context.Acting.Any(a => a.ActorId == createActing.ActorId && a.MovieId == createActing.MovieId);
    }

    /// <inheritdoc />
    public void DeleteActing(int id)
    {
        var acting = Context.Acting.Find(id) ??
                     throw new BadHttpRequestException($"Acting with id = {id} does not exist.");

        Context.Acting.Remove(acting);
        Context.SaveChanges();
    }

    /// <inheritdoc />
    public List<MovieDto> GetMoviesForActor(Guid actorId)
    {
        return Context.Acting.Where(a => a.ActorId == actorId).Select(a => Mapper.Map<MovieDto>(a)).ToList();
    }

    /// <inheritdoc />
    public List<ActingDto> GetActings()
    {
        return Context.Acting.Select(a => Mapper.Map<ActingDto>(a)).ToList();
    }
}