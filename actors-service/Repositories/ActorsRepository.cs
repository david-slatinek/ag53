using actors_service.Data;
using actors_service.Interfaces;
using actors_service.Models.Database;
using actors_service.Models.Filters;
using actors_service.Models.Requests;
using actors_service.Models.Responses;
using AutoMapper;

namespace actors_service.Repositories;

/// <summary>
/// Actors repository.
/// </summary>
/// <param name="context">Database context.</param>
/// <param name="mapper">Mapper.</param>
public class ActorsRepository(DataContext context, IMapper mapper) : IActorsRepository
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

        var actor = Mapper.Map<Actor>(createActor);
        actor.Id = Guid.NewGuid();

        Context.Actors.Add(actor);
        Context.SaveChanges();

        return Mapper.Map<ActorDto>(actor);
    }

    /// <inheritdoc />
    public ActorDto GetActorById(Guid id)
    {
        var actor = Context.Actors.Find(id) ?? throw new BadHttpRequestException($"Actor with id = {id} not found.");
        return Mapper.Map<ActorDto>(actor);
    }

    /// <inheritdoc />
    public ActorDto UpdateActor(Guid id, UpdateActor actor)
    {
        var birthDate = DateOnly.Parse(actor.BirthDate);
        if (birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BadHttpRequestException("Birth date cannot be in the future.");
        }

        var actorToUpdate = Context.Actors.Find(id) ??
                            throw new BadHttpRequestException($"Actor with id = {id} not found.");

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

        return Mapper.Map<ActorDto>(actorToUpdate);
    }

    /// <inheritdoc />
    public void DeleteActor(Guid id)
    {
        var actor = Context.Actors.Find(id) ?? throw new BadHttpRequestException($"Actor with id = {id} not found.");
        Context.Actors.Remove(actor);
        Context.SaveChanges();
    }

    /// <inheritdoc />
    public List<ActorDto> GetActors()
    {
        return Context.Actors.Select(a => Mapper.Map<ActorDto>(a)).ToList();
    }

    /// <inheritdoc />
    public PagedActors GetPagedActors(PaginationFilter paginationFilter)
    {
        var actors = Context.Actors.Select(a => Mapper.Map<ActorDto>(a))
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToList();

        var totalRecords = Context.Actors.Count();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)paginationFilter.PageSize);

        if (paginationFilter.PageNumber > totalPages && totalPages > 0)
        {
            throw new BadHttpRequestException("Page number is greater than total pages.");
        }

        return new PagedActors
        {
            Actors = actors,
            PageNumber = paginationFilter.PageNumber,
            PageSize = paginationFilter.PageSize,
            TotalPages = totalPages,
            TotalRecords = totalRecords
        };
    }
}