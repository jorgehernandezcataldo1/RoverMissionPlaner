using RoverMissionPlanner.Domain.Entities;

namespace RoverMissionPlanner.Application.Commands;

public class CreateRoverTaskCommand
{
    public string RoverName { get; set; } = string.Empty;
    public TaskType TaskType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime StartsAt { get; set; } // UTC
    public int DurationMinutes { get; set; }
}
