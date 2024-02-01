using acting.Models.Database;
using acting.Models.Requests;
using acting.Models.Responses;
using AutoMapper;

namespace acting.Mappings;

/// <summary>
/// Mapping profile for acting.
/// </summary>
public class ActingProfile : Profile
{
    /// <summary>
    /// Create a new mapping profile for acting.
    /// </summary>
    public ActingProfile()
    {
        CreateMap<CreateActing, Acting>();
        CreateMap<Acting, ActingDto>();
        CreateMap<Acting, MovieDto>().ForMember(m => m.Id,
            opt => opt.MapFrom(a => a.MovieId));
    }
}