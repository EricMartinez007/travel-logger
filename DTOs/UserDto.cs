namespace TravelLogger.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public List<LogDto> Logs { get; set; } = new();
    public List<RecommendationDto> Recommendations { get; set; } = new();
}
