namespace MarsRover.Domain.Entities;

public enum Direction
{
    North,
    East,
    South,
    West
}

public static class DirectionExtensions
{
    public static string AsMapDrawSymbol(this Direction direction) =>
        direction switch
        {
            Direction.North => "^",
            Direction.East => ">",
            Direction.South => "v",
            Direction.West => "<",
            _ => throw new InvalidOperationException(
                $"invalid {nameof(direction)} {direction:G}")
        };
}