
using System.ComponentModel.DataAnnotations;

namespace TravelLogger.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Email  { get; set; }
    public string Description { get; set; }
    public string ImageUrl  { get; set; }
    public List<Logs> Logs { get; set; } = new();
    public List<Recommendation> Recommendations { get; set; } = new();
}