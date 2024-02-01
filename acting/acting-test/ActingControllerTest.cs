using acting.Controllers;
using acting.Interfaces;
using acting.Mappings;
using acting.Mocking;
using acting.Models.Requests;
using acting.Models.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace acting_test;

/// <summary>
/// Test acting controller.
/// </summary>
public class ActingControllerTest
{
    private readonly ActingController _actingController;

    /// <summary>
    /// Constructor.
    /// </summary>
    public ActingControllerTest()
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ActingProfile())).CreateMapper();
        IActingRepository repository = new ActingRepositoryFake(mapper);
        IActingService service = new ActingServiceFake(repository);
        _actingController = new ActingController(service, repository);
    }

    /// <summary>
    /// Create an acting.
    /// </summary>
    /// <returns>A created acting.</returns>
    private ActingDto CreateActing()
    {
        var createActing = new CreateActing
        {
            ActorId = Guid.NewGuid(),
            MovieId = Guid.NewGuid()
        };

        var result = _actingController.CreateActing(createActing);
        var createdActingResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdActingDto = Assert.IsType<ActingDto>(createdActingResult.Value);

        return createdActingDto;
    }

    [Fact]
    public void TestCreateActing()
    {
        var createdActingDto = CreateActing();

        Assert.True(createdActingDto.Id > 0);
        Assert.NotEqual(Guid.Empty, createdActingDto.ActorId);
        Assert.NotEqual(Guid.Empty, createdActingDto.MovieId);
    }

    [Fact]
    public void TestDeleteActing()
    {
        var createdActingDto = CreateActing();

        var result = _actingController.DeleteActing(createdActingDto.Id);
        var deletedActingResult = Assert.IsType<NoContentResult>(result);

        Assert.Equal(204, deletedActingResult.StatusCode);
    }

    [Fact]
    public void TestGetMoviesForActor()
    {
        var createdActingDto = CreateActing();

        var result = _actingController.GetMoviesForActor(createdActingDto.ActorId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var moviesDto = Assert.IsType<List<MovieDto>>(okResult.Value);

        Assert.Single(moviesDto);
        Assert.Equal(createdActingDto.MovieId, moviesDto[0].Id);
    }

    [Fact]
    public void TestGetActings()
    {
        var createdActingDto = CreateActing();
        var createdActingDto2 = CreateActing();

        var result = _actingController.GetActings();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actingsDto = Assert.IsType<List<ActingDto>>(okResult.Value);

        Assert.Equal(2, actingsDto.Count);
        Assert.Equal(createdActingDto.Id, actingsDto[0].Id);
        Assert.Equal(createdActingDto.ActorId, actingsDto[0].ActorId);

        Assert.Equal(createdActingDto2.Id, actingsDto[1].Id);
        Assert.Equal(createdActingDto2.ActorId, actingsDto[1].ActorId);
    }
}