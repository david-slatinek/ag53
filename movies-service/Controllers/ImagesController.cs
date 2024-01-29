using Microsoft.AspNetCore.Mvc;
using movies_service.Filters;
using movies_service.Interfaces;
using movies_service.Models.Requests;
using movies_service.Models.Responses;

namespace movies_service.Controllers;

/// <summary>
/// Images controller.
/// </summary>
/// <param name="imagesService">Images service.</param>
[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "Default")]
[Produces("application/json")]
public class ImagesController(IImagesService imagesService) : Controller
{
    /// <summary>
    /// Images service.
    /// </summary>
    private IImagesService ImagesService { get; } = imagesService;

    /// <summary>
    /// Upload images for a movie.
    /// </summary>
    /// <param name="movieId">Movie ID.</param>
    /// <param name="images">Images to upload.</param>
    /// <returns>HTTP response.</returns>
    /// <response code="201">Images were uploaded.</response>
    /// <response code="400">Files were not provided or movie does not exist.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("{movieId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
    [Consumes("multipart/form-data")]
    [ValidateImages]
    public async Task<IActionResult> UploadImage(Guid movieId, [FromForm] UploadImages images)
    {
        try
        {
            var imageIds = await ImagesService.AddImages(movieId, images.Images.ToList());
            return CreatedAtAction(nameof(UploadImage), new { imageIds[0].Id }, imageIds);
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