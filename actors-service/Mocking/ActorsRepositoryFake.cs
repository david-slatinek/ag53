using actors_service.Interfaces;
using actors_service.Models.Database;
using actors_service.Models.Filters;
using actors_service.Models.Requests;
using actors_service.Models.Responses;

namespace actors_service.Mocking;

/// <summary>
/// Repository used for unit testing.
/// </summary>
public class ActorsRepositoryFake : IActorsRepository
{
    /// <summary>
    /// Actors list.
    /// </summary>
    private List<Actor> _actors = [];

    /// <inheritdoc />
    public ActorDto CreateActor(CreateActor createActor)
    {
        var actor = new Actor
        {
            Id = Guid.NewGuid(),
            FirstName = createActor.FirstName,
            LastName = createActor.LastName,
            BirthDate = DateOnly.Parse(createActor.BirthDate)
        };

        _actors.Add(actor);

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
        var actor = _actors.FirstOrDefault(a => a.Id == id) ?? throw new BadHttpRequestException("Actor not found.");

        return new ActorDto
        {
            Id = actor.Id,
            FirstName = actor.FirstName,
            LastName = actor.LastName,
            BirthDate = actor.BirthDate.ToString("yyyy-MM-dd"),
        };
    }

    /// <inheritdoc />
    public ActorDto UpdateActor(Guid id, UpdateActor actor)
    {
        var actorToUpdate = _actors.FirstOrDefault(a => a.Id == id) ??
                            throw new BadHttpRequestException("Actor not found.");

        actorToUpdate.FirstName = actor.FirstName;
        actorToUpdate.LastName = actor.LastName;
        actorToUpdate.BirthDate = DateOnly.Parse(actor.BirthDate);

        // update the actor in the list
        _actors = _actors.Select(a => a.Id == id ? actorToUpdate : a).ToList();

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
        var actor = _actors.FirstOrDefault(a => a.Id == id) ??
                    throw new BadHttpRequestException("Actor not found.");

        _actors.Remove(actor);
    }

    /// <inheritdoc />
    public List<ActorDto> GetActors()
    {
        return _actors.Select(a => new ActorDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            BirthDate = a.BirthDate.ToString("yyyy-MM-dd")
        }).ToList();
    }

    /// <inheritdoc />
    public PagedActors GetPagedActors(PaginationFilter paginationFilter)
    {
        var actors = _actors.Select(a => new ActorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BirthDate = a.BirthDate.ToString("yyyy-MM-dd")
            })
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToList();

        var totalRecords = _actors.Count;
        var totalPages = (int)Math.Ceiling(totalRecords / (double)paginationFilter.PageSize);

        var pagedActors = new PagedActors
        {
            Actors = actors,
            PageNumber = paginationFilter.PageNumber,
            PageSize = paginationFilter.PageSize,
            TotalPages = totalPages,
            TotalRecords = _actors.Count
        };

        return pagedActors;
    }
}