using System.ComponentModel.DataAnnotations;

namespace TravelLogger.Models;

public class City
{
    public int Id { get; set; }
    public string Name {get; set; }
    public List<Logs> Logs { get; set; } = new();
    public List<Recommendation> Recommendations { get; set; } = new();
    public List<User> Users { get; set;} =new();
}