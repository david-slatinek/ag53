using actors_service.Data;
using actors_service.Interfaces;
using actors_service.Models.Database;
using actors_service.Models.Requests;
using actors_service.Models.Responses;

namespace actors_service.Repositories;

/// <summary>
/// Actors repository.
/// </summary>
/// <param name="context">Database context.</param>
public class ActorsRepository(DataContext context) : IActorsRepository
{
    /// <summary>
    /// Database context.
    /// </summary>
    private DataContext Context { get; } = context;

    /// <inheritdoc />
    public ActorDto CreateActor(CreateActor createActor)
    {
        var birthDate = DateOnly.Parse(createActor.BirthDate);
        if (birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BadHttpRequestException("Birth date cannot be in the future.");
        }

        var actorExists = Context.Actors.Any(a =>
            a.FirstName == createActor.FirstName && a.LastName == createActor.LastName &&
            a.BirthDate == birthDate);

        if (actorExists)
        {
            throw new BadHttpRequestException("Actor already exists.");
        }

        var actor = new Actor
        {
            Id = Guid.NewGuid(),
            FirstName = createActor.FirstName,
            LastName = createActor.LastName,
            BirthDate = DateOnly.Parse(createActor.BirthDate)
        };

        Context.Actors.Add(actor);
        Context.SaveChanges();

        return new ActorDto
        {
            Id = actor.Id,
            FirstName = actor.FirstName,
            LastName = actor.LastName,
            BirthDate = actor.BirthDate.ToString("yyyy-MM-dd")
        };
    }

    /// <inheritdoc />
    public ActorDto GetActorById(Guid id)
    {
        var actor = Context.Actors.Find(id) ?? throw new BadHttpRequestException("Actor not found.");

        return new ActorDto
        {
            Id = actor.Id,
            FirstName = actor.FirstName,
            LastName = actor.LastName,
            BirthDate = actor.BirthDate.ToString("yyyy-MM-dd")
        };
    }

    /// <inheritdoc />
    public ActorDto UpdateActor(Guid id, UpdateActor actor)
    {
        var birthDate = DateOnly.Parse(actor.BirthDate);
        if (birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BadHttpRequestException("Birth date cannot be in the future.");
        }

        var actorToUpdate = Context.Actors.Find(id) ?? throw new BadHttpRequestException("Actor not found.");

        var actorExists = Context.Actors.Any(a =>
            a.FirstName == actor.FirstName && a.LastName == actor.LastName &&
            a.BirthDate == birthDate);

        if (actorExists)
        {
            throw new BadHttpRequestException("Actor already exists.");
        }

        actorToUpdate.FirstName = actor.FirstName;
        actorToUpdate.LastName = actor.LastName;
        actorToUpdate.BirthDate = birthDate;

        Context.SaveChanges();

        return new ActorDto
        {
            Id = actorToUpdate.Id,
            FirstName = actorToUpdate.FirstName,
            LastName = actorToUpdate.LastName,
            BirthDate = actorToUpdate.BirthDate.ToString("yyyy-MM-dd")
        };
    }

    /// <inheritdoc />
    public void DeleteActor(Guid id)
    {
        var actor = Context.Actors.Find(id) ?? throw new BadHttpRequestException("Actor not found.");
        Context.Actors.Remove(actor);
        Context.SaveChanges();
    }

    /// <inheritdoc />
    public List<ActorDto> GetActors()
    {
        return Context.Actors.Select(a => new ActorDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            BirthDate = a.BirthDate.ToString("yyyy-MM-dd")
        }).ToList();
    }
}