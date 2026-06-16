namespace TravelLogger.DTOs;

public class RecommendationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CityId { get; set; }
    public string Description { get; set; }
    public List<UpvoteDto> Upvotes { get; set; } = new();
}
