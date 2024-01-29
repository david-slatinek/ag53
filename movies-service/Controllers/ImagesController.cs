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
    /// <returns>Created image IDs.</returns>
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

    /// <summary>
    /// Get image by ID.
    /// </summary>
    /// <param name="id">Image ID.</param>
    /// <returns>Image.</returns>
    /// <response code="200">Image.</response>
    /// <response code="400">Image does not exist.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
    public async Task<IActionResult> GetImage(int id)
    {
        try
        {
            var imageData = await ImagesService.GetImage(id);
            return File(imageData.Stream, imageData.ContentType, imageData.FileName);
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
    /// Delete image by ID.
    /// </summary>
    /// <param name="id">Image ID.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Image was deleted.</response>
    /// <response code="400">Image does not exist.</response>
    /// <response code="500">Internal server error.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
    public async Task<IActionResult> DeleteImage(int id)
    {
        try
        {
            await ImagesService.DeleteImage(id);
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