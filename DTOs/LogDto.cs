namespace TravelLogger.DTOs;

public class LogDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CityId { get; set; }
    public string Comments { get; set; }
    public DateTime Date { get; set; }
}
