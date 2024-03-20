using MarsRover.Domain.Entities;

namespace MarsRover.API.Controllers.Mission.Dtos;

public class LandingRequestDto
{
    public int X { get; set; }

    public int Y { get; set; }

    public Direction Direction { get; set; }
}
