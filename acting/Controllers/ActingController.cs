using acting.Interfaces;
using acting.Models.Database;
using acting.Models.Requests;
using acting.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace acting.Controllers;

/// <summary>
/// Acting controller.
/// </summary>
/// <param name="actingService">Acting service.</param>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class ActingController(IActingService actingService) : Controller
{
    /// <summary>
    /// Acting service.
    /// </summary>
    private IActingService ActingService { get; } = actingService;

    /// <summary>
    /// Add an actor to a movie.
    /// </summary>
    /// <param name="createActing">Acting data.</param>
    /// <returns>Created acting.</returns>
    /// <response code="201">Returns the newly created acting.</response>
    /// <response code="400">If the acting data is invalid.</response>
    /// <response code="500">If there was an error creating the acting.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Acting))]
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
}