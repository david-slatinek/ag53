using actors_service.Models.Database;
using actors_service.Models.Requests;
using actors_service.Models.Responses;
using AutoMapper;

namespace actors_service.Mappings;

/// <summary>
/// Mapping profile for actors.
/// </summary>
public class ActorProfile : Profile
{
    /// <summary>
    /// Create the mapping profile.
    /// </summary>
    public ActorProfile()
    {
        CreateMap<Actor, ActorDto>().ForMember(
            dest => dest.BirthDate,
            opt => opt.MapFrom(src => src.BirthDate.ToString("yyyy-MM-dd"))
        );
        CreateMap<CreateActor, Actor>().ForMember(
            dest => dest.BirthDate,
            opt => opt.MapFrom(src => DateOnly.Parse(src.BirthDate))
        );
    }
}