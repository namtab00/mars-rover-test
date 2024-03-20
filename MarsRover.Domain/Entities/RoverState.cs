namespace MarsRover.Domain.Entities;

public record RoverState(MapCoordinate Position, Direction Direction)
{
    public override string? ToString()
    {
        return $"({Position.X},{Position.Y}) due {Direction:G}";
    }
}
