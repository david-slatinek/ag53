using acting.Data;
using acting.Interfaces;
using acting.Models.Database;
using acting.Models.Requests;

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
}