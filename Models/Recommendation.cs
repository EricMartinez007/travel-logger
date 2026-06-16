using System.ComponentModel.DataAnnotations;

namespace TravelLogger.Models;

public class Recommendation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CityId { get; set; }
    public string Description {get; set; }
}