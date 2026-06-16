using TravelLogger.Models;

namespace TravelLogger.DTOs;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<LogDto> Logs { get; set; } = new();
    public List<RecommendationDto> Recommendations { get; set; } = new();
    public List<UserDto> Users { get; set;} =new();
}
