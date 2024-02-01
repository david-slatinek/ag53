using actors_service.Interfaces;
using actors_service.Models.Database;
using actors_service.Models.Filters;
using actors_service.Models.Requests;
using actors_service.Models.Responses;
using AutoMapper;

namespace actors_service.Mocking;

/// <summary>
/// Repository used for unit testing.
/// <param name="mapper">Mapper.</param>
/// </summary>
public class ActorsRepositoryFake(IMapper mapper) : IActorsRepository
{
    /// <summary>
    /// Actors list.
    /// </summary>
    private List<Actor> _actors = [];

    /// <summary>
    /// Mapper.
    /// </summary>
    private IMapper Mapper { get; } = mapper;

    /// <inheritdoc />
    public ActorDto CreateActor(CreateActor createActor)
    {
        var actor = Mapper.Map<Actor>(createActor);
        actor.Id = Guid.NewGuid();

        _actors.Add(actor);

        return Mapper.Map<ActorDto>(actor);
    }

    /// <inheritdoc />
    public ActorDto GetActorById(Guid id)
    {
        var actor = _actors.FirstOrDefault(a => a.Id == id) ??
                    throw new BadHttpRequestException($"Actor with id = {id} not found.");
        return Mapper.Map<ActorDto>(actor);
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

        return Mapper.Map<ActorDto>(actorToUpdate);
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
        return _actors.Select(a => Mapper.Map<ActorDto>(a)).ToList();
    }

    /// <inheritdoc />
    public PagedActors GetPagedActors(PaginationFilter paginationFilter)
    {
        var actors = _actors.Select(a => Mapper.Map<ActorDto>(a))
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