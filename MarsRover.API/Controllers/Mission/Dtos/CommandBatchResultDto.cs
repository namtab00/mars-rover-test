namespace MarsRover.API.Controllers.Mission.Dtos;

public class CommandBatchResultDto
{
    public required RoverStateResultDto RoverState { get; set; }

    public MapCoordinateResultDto? EncounteredObstacleAt { get; set; }

    public List<CommandResultDto> CommandResults { get; set; } = new();
}
