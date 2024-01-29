using acting.Data;
using acting.Interfaces;
using acting.Models.Database;
using acting.Models.Requests;
using acting.Models.Responses;

namespace acting.Repositories;

/// <summary>
/// Acting repository.
/// </summary>
/// <param name="context">Database context.</param>
public class ActingRepository(DataContext context) : IActingRepository
{
    /// <summary>
    /// Database context.
    /// </summary>
    private DataContext Context { get; } = context;

    /// <inheritdoc />
    public Acting CreateActing(CreateActing createActing)
    {
        var acting = new Acting
        {
            ActorId = createActing.ActorId,
            MovieId = createActing.MovieId
        };

        Context.Acting.Add(acting);
        Context.SaveChanges();

        return acting;
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
        return Context.Acting.Where(a => a.ActorId == actorId).Select(a => new MovieDto
        {
            Id = a.MovieId
        }).ToList();
    }

    /// <inheritdoc />
    public List<ActingDto> GetActings()
    {
        return Context.Acting.Select(a => new ActingDto
        {
            Id = a.Id,
            ActorId = a.ActorId,
            MovieId = a.MovieId
        }).ToList();
    }
}