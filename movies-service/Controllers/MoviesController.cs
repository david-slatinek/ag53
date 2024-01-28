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

    /// <summary>
    /// Update a movie.
    /// </summary>
    /// <param name="id">Movie id.</param>
    /// <param name="updateMovie">Movie data.</param>
    /// <returns>Updated movie.</returns>
    /// <response code="200">Returns the updated movie.</response>
    /// <response code="400">If the movie data is invalid.</response>
    /// <response code="500">If there was an error updating the movie.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateMovie(Guid id, [FromBody] UpdateMovie updateMovie)
    {
        try
        {
            var movie = MoviesRepository.UpdateMovie(id, updateMovie);
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

    /// <summary>
    /// Delete a movie.
    /// </summary>
    /// <param name="id">Movie id.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Movie deleted.</response>
    /// <response code="400">If movie is not found.</response>
    /// <response code="500">If there was an error deleting the movie.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteMovie(Guid id)
    {
        try
        {
            MoviesRepository.DeleteMovie(id);
            return NoContent();
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