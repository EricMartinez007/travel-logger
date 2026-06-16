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
        CreateMap<UserDto, User>();
        CreateMap<City, CityDto>();
        CreateMap<CityDto, City>();
        CreateMap<Logs, LogDto>();
        CreateMap<LogDto, Logs>();
        CreateMap<Recommendation, RecommendationDto>();
        CreateMap<RecommendationDto, Recommendation>();
        CreateMap<Upvote, UpvoteDto>();
        CreateMap<UpvoteDto, Upvote>();
    }
}
