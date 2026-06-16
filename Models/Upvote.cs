using System.ComponentModel.DataAnnotations;

namespace TravelLogger.Models;

public class Upvote
{
    public int Id { get; set; }
    public int RecommendationId { get; set; }
    public int UserId { get; set; }
}