using Microsoft.AspNetCore.Mvc;
using movies_service.Interfaces;
using movies_service.Models.Requests;
using movies_service.Models.Responses;

namespace movies_service.Controllers;

/// <summary>
/// Movies controller.
/// </summary>
/// <param name="moviesRepository">Movies repository.</param>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class MoviesController(IMoviesRepository moviesRepository) : Controller
{
    /// <summary>
    /// Movies repository.
    /// </summary>
    private IMoviesRepository MoviesRepository { get; } = moviesRepository;

    /// <summary>
    /// Creates a movie.
    /// </summary>
    /// <param name="createMovie">Movie data.</param>
    /// <returns>Create movie.</returns>
    /// <response code="201">Returns the newly created movie.</response>
    /// <response code="400">If the movie data is invalid.</response>
    /// <response code="500">If there was an error creating the movie.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateMovie([FromBody] CreateMovie createMovie)
    {
        try
        {
            var createdMovie = MoviesRepository.CreateMovie(createMovie);
            return CreatedAtAction(nameof(CreateMovie), new { id = createdMovie.Id }, createdMovie);
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new Error
            {
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Error
            {
                Message = e.Message
            });
        }
    }

    /// <summary>
    /// Get movie by id.
    /// </summary>
    /// <param name="id">Movie id.</param>
    /// <returns>Movie.</returns>
    /// <response code="200">Returns the movie.</response>
    /// <response code="400">If movie is not found.</response>
    /// <response code="500">If there was an error getting the movie.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetMovie(Guid id)
    {
        try
        {
            var movie = MoviesRepository.GetMovie(id);
            return Ok(movie);
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new Error
            {
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Error
            {
                Message = e.Message
            });
        }
    }
}