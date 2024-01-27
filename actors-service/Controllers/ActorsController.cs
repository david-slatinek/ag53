using actors_service.Interfaces;
using actors_service.Models.Database;
using actors_service.Models.Requests;
using actors_service.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace actors_service.Controllers;

/// <summary>
/// Actors controller.
/// </summary>
/// <param name="actorsRepository">Actors repository.</param>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ActorsController(IActorsRepository actorsRepository) : Controller
{
    /// <summary>
    /// Actors repository.
    /// </summary>
    private IActorsRepository ActorsRepository { get; } = actorsRepository;

    /// <summary>
    /// Creates an actor.
    /// </summary>
    /// <param name="createActor">Actor data.</param>
    /// <returns>Created actor.</returns>
    /// <response code="201">Returns the newly created actor.</response>
    /// <response code="400">If the actor data is invalid.</response>
    /// <response code="500">If there was an error creating the actor.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Actor))]
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
}