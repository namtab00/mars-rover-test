namespace MarsRover.API.Controllers.Mission.Dtos;

public class MissionStatusResultDto
{
    public RoverStateResultDto? RoverState { get; set; }

    public int TotalKnownObstacleCount { get; set; }
}
