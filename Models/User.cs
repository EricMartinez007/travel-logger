
using System.ComponentModel.DataAnnotations;

namespace TravelLogger.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Email  { get; set; }
    public string Description { get; set; }
    public string ImageUrl  { get; set; }

}