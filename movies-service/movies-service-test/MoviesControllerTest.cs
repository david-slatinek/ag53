using Microsoft.AspNetCore.Mvc;
using movies_service.Controllers;
using movies_service.Interfaces;
using movies_service.Mocking;
using movies_service.Models.Requests;
using movies_service.Models.Responses;

namespace movies_service_test;

/// <summary>
/// Test movies controller.
/// </summary>
public class MoviesControllerTest
{
    /// <summary>
    /// Movies controller.
    /// </summary>
    private readonly MoviesController _controller;

    /// <summary>
    /// Constructor.
    /// </summary>
    public MoviesControllerTest()
    {
        IMoviesRepository repository = new MoviesRepositoryFake();
        _controller = new MoviesController(repository);
    }

    /// <summary>
    /// Create a movie.
    /// </summary>
    /// <returns>Object containing the movie data and the created movie.</returns>
    private (CreateMovie, MovieDto) CreateMovie()
    {
        var createMovie = new CreateMovie
        {
            Title = "Movie",
            Description = "Description",
            Release = "2021-01-01"
        };

        var result = _controller.CreateMovie(createMovie);
        var createdMovieResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdMovieDto = Assert.IsType<MovieDto>(createdMovieResult.Value);

        return (createMovie, createdMovieDto);
    }

    /// <summary>
    /// Test create movie endpoint.
    /// </summary>
    [Fact]
    public void TestCreateMovie()
    {
        var (createMovie, movieDto) = CreateMovie();

        Assert.NotEqual(Guid.Empty, movieDto.Id);
        Assert.Equal(createMovie.Title, movieDto.Title);
        Assert.Equal(createMovie.Description, movieDto.Description);
        Assert.Equal(createMovie.Release, movieDto.Release);
    }

    /// <summary>
    /// Test get movie by id endpoint.
    /// </summary>
    [Fact]
    public void TestGetMovieById()
    {
        // Arrange
        var (_, movieDto) = CreateMovie();

        // Act
        var result = _controller.GetMovie(movieDto.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMovieDto = Assert.IsType<MovieDto>(okResult.Value);

        Assert.Equal(movieDto.Id, returnedMovieDto.Id);
        Assert.Equal(movieDto.Title, returnedMovieDto.Title);
        Assert.Equal(movieDto.Description, returnedMovieDto.Description);
        Assert.Equal(movieDto.Release, returnedMovieDto.Release);
    }
    
    /// <summary>
    /// Test update movie endpoint.
    /// </summary>
    [Fact]
    public void TestUpdateMovie()
    {
        // Arrange
        var (_, movieDto) = CreateMovie();
        var updateMovie = new UpdateMovie
        {
            Title = "Updated Movie",
            Description = "Updated Description",
            Release = "2021-01-01"
        };

        // Act
        var result = _controller.UpdateMovie(movieDto.Id, updateMovie);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMovieDto = Assert.IsType<MovieDto>(okResult.Value);

        Assert.Equal(movieDto.Id, returnedMovieDto.Id);
        Assert.Equal(updateMovie.Title, returnedMovieDto.Title);
        Assert.Equal(updateMovie.Description, returnedMovieDto.Description);
        Assert.Equal(updateMovie.Release, returnedMovieDto.Release);
    }
}