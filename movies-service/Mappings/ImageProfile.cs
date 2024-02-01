using AutoMapper;
using movies_service.Models.Database;
using movies_service.Models.Responses;

namespace movies_service.Mappings;

/// <summary>
/// Mapping profile for images.
/// </summary>
public class ImageProfile : Profile
{
    /// <summary>
    /// Create the mapping profile.
    /// </summary>
    public ImageProfile()
    {
        CreateMap<Image, ImageDto>();
    }
}