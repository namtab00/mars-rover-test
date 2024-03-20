using MarsRover.Domain.Entities;

namespace MarsRover.API.Controllers.Mission.Dtos;

public class RoverStateResultDto
{
    public required MapCoordinateResultDto Position { get; set; }

    public Direction Direction { get; set; }
}
