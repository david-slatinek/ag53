using AutoMapper;
using movies_service.Models.Database;
using movies_service.Models.Requests;
using movies_service.Models.Responses;

namespace movies_service.Mappings;

/// <summary>
/// Mapping profile for movies.
/// </summary>
public class MovieProfile : Profile
{
    /// <summary>
    /// Create the mapping profile.
    /// </summary>
    public MovieProfile()
    {
        CreateMap<Movie, MovieDto>().ForMember(
            dest => dest.Release,
            opt => opt.MapFrom(src => src.Release.ToString("yyyy-MM-dd"))
        );
        CreateMap<CreateMovie, Movie>().ForMember(
            dest => dest.Release,
            opt => opt.MapFrom(src => DateOnly.Parse(src.Release))
        );
    }
}