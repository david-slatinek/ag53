using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using movies_service.Models.Requests;
using movies_service.Models.Responses;

namespace movies_service.Filters;

/// <summary>
/// Filter for validating images.
/// </summary>
public class ValidateImages : ActionFilterAttribute
{
    /// <summary>
    /// Validate images.
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments["images"] is not UploadImages images || images.Images.Count == 0)
        {
            context.Result = new BadRequestObjectResult(new Error
            {
                Message = "Images are required."
            });
            return;
        }

        if (images.Images.Count > 10)
        {
            context.Result = new BadRequestObjectResult(new Error
            {
                Message = "Cannot upload more than 10 images."
            });
            return;
        }

        if (images.Images.Any(image => image.ContentType != "image/jpeg" && image.ContentType != "image/png"))
        {
            context.Result = new BadRequestObjectResult(new Error
            {
                Message = "Image must be a JPEG or PNG."
            });
            return;
        }

        if (images.Images.Any(image => image.Length > 10_000_000))
        {
            context.Result = new BadRequestObjectResult(new Error
            {
                Message = "Image size must be less than 10MB."
            });
            return;
        }

        HashSet<string> imageHashes = [];
        using var shaAlg = SHA256.Create();

        foreach (var image in images.Images)
        {
            var hash = shaAlg.ComputeHash(image.OpenReadStream());
            var hashString = Convert.ToBase64String(hash);

            if (!imageHashes.Add(hashString))
            {
                context.Result = new BadRequestObjectResult(new Error
                {
                    Message = "Images must be unique."
                });
                return;
            }
        }

        base.OnActionExecuting(context);
    }
}