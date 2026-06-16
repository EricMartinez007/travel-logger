using AutoMapper;
using TravelLogger.DTOs;
using TravelLogger.Models;
namespace TravelLogger;

// Tells AutoMapper how to convert each EF entity into its matching DTO.
// Properties with the same name (Id, Email, etc.) are mapped automatically.
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>();
        CreateMap<City, CityDto>();
        CreateMap<Logs, LogDto>();
        CreateMap<Recommendation, RecommendationDto>();
        CreateMap<Upvote, UpvoteDto>();
    }
}
