using actors_service.Interfaces;
using actors_service.Models.Filters;
using actors_service.Models.Requests;
using actors_service.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace actors_service.Controllers;

/// <summary>
/// Actors controller.
/// </summary>
/// <param name="actorsRepository">Actors repository.</param>1
/// <param name="moviesService">Movies service.</param>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class ActorsController(IActorsRepository actorsRepository, IMoviesService moviesService) : Controller
{
    /// <summary>
    /// Actors repository.
    /// </summary>
    private IActorsRepository ActorsRepository { get; } = actorsRepository;

    /// <summary>
    /// Movies service.
    /// </summary>
    private IMoviesService MoviesService { get; } = moviesService;

    /// <summary>
    /// Creates an actor.
    /// </summary>
    /// <param name="createActor">Actor data.</param>
    /// <returns>Created actor.</returns>
    /// <response code="201">Returns the newly created actor.</response>
    /// <response code="400">If the actor data is invalid.</response>
    /// <response code="500">If there was an error creating the actor.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActorDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateActor([FromBody] CreateActor createActor)
    {
        try
        {
            var createdActor = ActorsRepository.CreateActor(createActor);
            return CreatedAtAction(nameof(CreateActor), new { id = createdActor.Id }, createdActor);
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
    /// Get actor by id.
    /// </summary>
    /// <param name="id">Actor id.</param>
    /// <returns>Actor.</returns>
    /// <response code="200">Returns the actor.</response>
    /// <response code="400">If actor is not found.</response>
    /// <response code="500">If there was an error getting the actor.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetActor(Guid id)
    {
        try
        {
            var actor = ActorsRepository.GetActorById(id);
            var movies = MoviesService.GetMoviesForActor(id);

            actor.Movies = movies;

            return Ok(actor);
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
    /// Updates an actor.
    /// </summary>
    /// <param name="id">Actor id.</param>
    /// <param name="updateActor">Actor data.</param>
    /// <returns>Updated actor.</returns>
    /// <response code="200">Returns the updated actor.</response>
    /// <response code="400">If the actor data is invalid.</response>
    /// <response code="500">If there was an error updating the actor.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateActor(Guid id, [FromBody] UpdateActor updateActor)
    {
        try
        {
            var actor = ActorsRepository.UpdateActor(id, updateActor);
            return Ok(actor);
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
    /// Deletes an actor.
    /// </summary>
    /// <param name="id">Actor id.</param>
    /// <returns>No content.</returns>
    /// <response code="204">No content.</response>
    /// <response code="400">If the actor is not found.</response>
    /// <response code="500">If there was an error deleting the actor.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteActor(Guid id)
    {
        try
        {
            ActorsRepository.DeleteActor(id);
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
    /// Get actors.
    /// </summary>
    /// <returns>A list of actors.</returns>
    /// <response code="200">Returns the actors.</response>
    /// <response code="204">If there are no actors.</response>
    /// <response code="500">If there was an error getting the actors.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ActorDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetActors()
    {
        try
        {
            var actors = ActorsRepository.GetActors();
            if (actors.Count == 0)
            {
                return NoContent();
            }

            foreach (var actor in actors)
            {
                var movies = MoviesService.GetMoviesForActor(actor.Id);
                actor.Movies = movies;
            }

            return Ok(actors);
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
    /// Get actors using pagination.
    /// </summary>
    /// <param name="filter">Pagination filter.</param>
    /// <returns>Paged actors.</returns>
    /// <response code="200">Returns the paged actors.</response>
    /// <response code="204">If there are no actors.</response>
    /// <response code="400">If the page number is invalid.</response>
    /// <response code="500">If there was an error getting the actors.</response>
    [HttpGet("paged")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedActors))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetPagedActors([FromQuery] PaginationFilter filter)
    {
        try
        {
            var actors = ActorsRepository.GetPagedActors(filter);

            if (actors.Actors.Count == 0)
            {
                return NoContent();
            }
            
            foreach (var actor in actors.Actors)
            {
                var movies = MoviesService.GetMoviesForActor(actor.Id);
                actor.Movies = movies;
            }

            return Ok(actors);
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