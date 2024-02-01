using actors_service.Controllers;
using actors_service.Interfaces;
using actors_service.Mappings;
using actors_service.Mocking;
using actors_service.Models.Filters;
using actors_service.Models.Requests;
using actors_service.Models.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace actors_service_test;

/// <summary>
/// Test actors controller.
/// </summary>
public class ActorsControllerTest
{
    private readonly ActorsController _controller;

    /// <summary>
    /// Constructor.
    /// </summary>
    public ActorsControllerTest()
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ActorProfile())).CreateMapper();
        IActorsRepository repository = new ActorsRepositoryFake(mapper);
        IMoviesService moviesService = new MoviesServiceFake();
        _controller = new ActorsController(repository, moviesService);
    }

    /// <summary>
    /// Create an actor.
    /// </summary>
    /// <returns>Object containing the actor data and the created actor.</returns>
    private (CreateActor, ActorDto) CreateActor()
    {
        var createActor = new CreateActor
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = "1980-01-01"
        };

        var result = _controller.CreateActor(createActor);
        var createdActorResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdActorDto = Assert.IsType<ActorDto>(createdActorResult.Value);

        return (createActor, createdActorDto);
    }

    /// <summary>
    /// Test create actor endpoint.
    /// </summary>
    [Fact]
    public void TestCreateActor()
    {
        var (createActor, actorDto) = CreateActor();

        Assert.NotEqual(Guid.Empty, actorDto.Id);
        Assert.Equal(createActor.FirstName, actorDto.FirstName);
        Assert.Equal(createActor.LastName, actorDto.LastName);
        Assert.Equal(createActor.BirthDate, actorDto.BirthDate);
    }

    /// <summary>
    /// Test get actor by id endpoint.
    /// </summary>
    [Fact]
    public void TestGetActorById()
    {
        // Arrange
        var (createActor, actorDto) = CreateActor();

        // Act
        var result = _controller.GetActor(actorDto.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actorDtoGet = Assert.IsType<ActorDto>(okResult.Value);

        Assert.NotEqual(Guid.Empty, actorDtoGet.Id);
        Assert.Equal(createActor.FirstName, actorDtoGet.FirstName);
        Assert.Equal(createActor.LastName, actorDtoGet.LastName);
        Assert.Equal(createActor.BirthDate, actorDtoGet.BirthDate);
    }

    /// <summary>
    /// Test update actor endpoint.
    /// </summary>
    [Fact]
    public void TestUpdateActor()
    {
        // Arrange
        var (_, actorDto) = CreateActor();

        var updateActor = new UpdateActor
        {
            FirstName = "Jane",
            LastName = "Doe",
            BirthDate = "1981-03-05"
        };

        // Act
        var result = _controller.UpdateActor(actorDto.Id, updateActor);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actorDtoUpdate = Assert.IsType<ActorDto>(okResult.Value);

        Assert.NotEqual(Guid.Empty, actorDtoUpdate.Id);
        Assert.Equal(updateActor.FirstName, actorDtoUpdate.FirstName);
        Assert.Equal(updateActor.LastName, actorDtoUpdate.LastName);
        Assert.Equal(updateActor.BirthDate, actorDtoUpdate.BirthDate);
    }

    /// <summary>
    /// Test delete actor endpoint.
    /// </summary>
    [Fact]
    public void TestDeleteActor()
    {
        // Arrange
        var (_, actorDto) = CreateActor();

        // Act
        var result = _controller.DeleteActor(actorDto.Id);

        // Assert
        var okResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, okResult.StatusCode);
    }

    /// <summary>
    /// Test get all actors endpoint.
    /// </summary>
    [Fact]
    public void TestGetAllActors()
    {
        // Arrange
        var (_, actorDto) = CreateActor();
        var (_, actorDto2) = CreateActor();

        // Act
        var result = _controller.GetActors();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actorDtos = Assert.IsType<List<ActorDto>>(okResult.Value);

        Assert.NotEmpty(actorDtos);
        Assert.Equal(2, actorDtos.Count);

        Assert.Equal(actorDto.Id, actorDtos[0].Id);
        Assert.Equal(actorDto.FirstName, actorDtos[0].FirstName);
        Assert.Equal(actorDto.LastName, actorDtos[0].LastName);
        Assert.Equal(actorDto.BirthDate, actorDtos[0].BirthDate);

        Assert.Equal(actorDto2.Id, actorDtos[1].Id);
        Assert.Equal(actorDto2.FirstName, actorDtos[1].FirstName);
        Assert.Equal(actorDto2.LastName, actorDtos[1].LastName);
        Assert.Equal(actorDto2.BirthDate, actorDtos[1].BirthDate);
    }

    /// <summary>
    /// Test get paged actors endpoint.
    /// </summary>
    [Fact]
    public void TestGetPagedActors()
    {
        // Arrange
        const int numberOfActors = 50;

        Enumerable.Range(1, numberOfActors).ToList().ForEach(i =>
        {
            var createActor = new CreateActor
            {
                FirstName = $"John {i}",
                LastName = $"Doe {i}",
                BirthDate = "1980-01-01"
            };

            _controller.CreateActor(createActor);
        });

        var paginationFilter = new PaginationFilter();

        // Act
        var result = _controller.GetPagedActors(paginationFilter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var pagedActors = Assert.IsType<PagedActors>(okResult.Value);

        Assert.NotEmpty(pagedActors.Actors);
        Assert.Equal(paginationFilter.PageSize, pagedActors.Actors.Count);

        Assert.Equal(1, pagedActors.PageNumber);
        Assert.Equal(20, pagedActors.PageSize);
        Assert.Equal(3, pagedActors.TotalPages);
        Assert.Equal(numberOfActors, pagedActors.TotalRecords);
    }
}