using acting.Interfaces;
using acting.Models.Requests;
using acting.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace acting.Controllers;

/// <summary>
/// Acting controller.
/// </summary>
/// <param name="actingService">Acting service.</param>
/// <param name="actingRepository">Acting repository.</param>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[ResponseCache(CacheProfileName = "Default")]
public class ActingController(IActingService actingService, IActingRepository actingRepository) : Controller
{
    /// <summary>
    /// Acting service.
    /// </summary>
    private IActingService ActingService { get; } = actingService;

    /// <summary>
    /// Acting repository.
    /// </summary>
    private IActingRepository ActingRepository { get; } = actingRepository;

    /// <summary>
    /// Add an actor to a movie.
    /// </summary>
    /// <param name="createActing">Acting data.</param>
    /// <returns>Created acting.</returns>
    /// <response code="201">Returns the newly created acting.</response>
    /// <response code="400">If the acting data is invalid.</response>
    /// <response code="500">If there was an error creating the acting.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActingDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
    public IActionResult CreateActing([FromBody] CreateActing createActing)
    {
        try
        {
            var createdActing = ActingService.CreateActing(createActing);
            return CreatedAtAction(nameof(CreateActing), new { id = createdActing.Id }, createdActing);
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
    /// Delete an acting.
    /// </summary>
    /// <param name="id">Acting ID.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the acting was deleted.</response>
    /// <response code="400">If acting was not found.</response>
    /// <response code="500">If there was an error deleting the acting.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
    public IActionResult DeleteActing(int id)
    {
        try
        {
            ActingRepository.DeleteActing(id);
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

    /// <summary>
    /// Get all movies for an actor.
    /// </summary>
    /// <param name="actorId">Actor ID.</param>
    /// <returns>List of movies for the actor.</returns>
    /// <response code="200">Returns the movies for the actor.</response>
    /// <response code="204">If there are no movies for the actor.</response>
    /// <response code="500">If there was an error getting the movies for the actor.</response>
    [HttpGet("actors/{actorId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MovieDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
    public IActionResult GetMoviesForActor(Guid actorId)
    {
        try
        {
            var movies = ActingRepository.GetMoviesForActor(actorId);
            if (movies.Count == 0)
            {
                return NoContent();
            }

            return Ok(movies);
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
    /// Get all actings.
    /// </summary>
    /// <returns>All actings.</returns>
    /// <response code="200">Returns all actings.</response>
    /// <response code="204">If there are no actings.</response>
    /// <response code="500">If there was an error getting the actings.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ActingDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
    public IActionResult GetActings()
    {
        try
        {
            var actings = ActingRepository.GetActings();
            if (actings.Count == 0)
            {
                return NoContent();
            }

            return Ok(actings);
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