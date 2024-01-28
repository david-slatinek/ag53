using Microsoft.AspNetCore.Mvc;
using movies_service.Controllers;
using movies_service.Interfaces;
using movies_service.Mocking;
using movies_service.Models.Filters;
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

    /// <summary>
    /// Test delete movie endpoint.
    /// </summary>
    [Fact]
    public void TestDeleteMovie()
    {
        // Arrange
        var (_, movieDto) = CreateMovie();

        // Act
        var result = _controller.DeleteMovie(movieDto.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    /// <summary>
    /// Test get all movies endpoint.
    /// </summary>
    [Fact]
    public void TestGetAllMovies()
    {
        // Arrange
        var (_, movieDto1) = CreateMovie();
        var (_, movieDto2) = CreateMovie();

        // Act
        var result = _controller.GetMovies();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var movieDtos = Assert.IsType<List<MovieDto>>(okResult.Value);

        Assert.NotEmpty(movieDtos);
        Assert.Equal(2, movieDtos.Count);

        Assert.Equal(movieDto1.Id, movieDtos[0].Id);
        Assert.Equal(movieDto1.Title, movieDtos[0].Title);
        Assert.Equal(movieDto1.Description, movieDtos[0].Description);
        Assert.Equal(movieDto1.Release, movieDtos[0].Release);

        Assert.Equal(movieDto2.Id, movieDtos[1].Id);
        Assert.Equal(movieDto2.Title, movieDtos[1].Title);
        Assert.Equal(movieDto2.Description, movieDtos[1].Description);
        Assert.Equal(movieDto2.Release, movieDtos[1].Release);
    }

    /// <summary>
    /// Test get paged movies endpoint.
    /// </summary>
    [Fact]
    public void TestGetPagedMovies()
    {
        // Arrange
        const int numberOfMovies = 50;

        Enumerable.Range(1, numberOfMovies).ToList().ForEach(i =>
        {
            var createMovie = new CreateMovie
            {
                Title = $"Movie {i}",
                Description = $"Description {i}",
                Release = "2021-01-01"
            };

            _controller.CreateMovie(createMovie);
        });

        var paginationFilter = new PaginationFilter();

        // Act
        var result = _controller.GetPagedMovies(paginationFilter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var pagedMovies = Assert.IsType<PagedMovies>(okResult.Value);

        Assert.NotEmpty(pagedMovies.Movies);
        Assert.Equal(paginationFilter.PageSize, pagedMovies.Movies.Count);

        Assert.Equal(1, pagedMovies.PageNumber);
        Assert.Equal(20, pagedMovies.PageSize);
        Assert.Equal(3, pagedMovies.TotalPages);
        Assert.Equal(numberOfMovies, pagedMovies.TotalRecords);
    }
    
    /// <summary>
    /// Test get movies by title endpoint.
    /// </summary>
    [Fact]
    public void TestGetMovieByTitle()
    {
        // Arrange
        var (_, movieDto1) = CreateMovie();
        var (_, movieDto2) = CreateMovie();

        // Act
        var result = _controller.GetMovieByTitle(movieDto1.Title);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var movieDtos = Assert.IsType<List<MovieDto>>(okResult.Value);

        Assert.NotEmpty(movieDtos);
        Assert.Equal(2, movieDtos.Count);

        Assert.Equal(movieDto1.Id, movieDtos[0].Id);
        Assert.Equal(movieDto1.Title, movieDtos[0].Title);
        Assert.Equal(movieDto1.Description, movieDtos[0].Description);
        Assert.Equal(movieDto1.Release, movieDtos[0].Release);

        Assert.Equal(movieDto2.Id, movieDtos[1].Id);
        Assert.Equal(movieDto2.Title, movieDtos[1].Title);
        Assert.Equal(movieDto2.Description, movieDtos[1].Description);
        Assert.Equal(movieDto2.Release, movieDtos[1].Release);
    }
}