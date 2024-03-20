using System.Text;
using MarsRover.Domain.Entities;

namespace MarsRover.Domain.Mission;

public record MissionStatus(MarsMap Map, RoverState? RoverState)
{
    public void IsValidLandingPositionOrThrow(MapCoordinate landingPosition)
    {
        const int minWidth = 0;
        var maxWidth = Map.Width;
        if (!Enumerable.Range(minWidth, maxWidth).Contains(landingPosition.X))
        {
            throw new InvalidOperationException(
                $"landing horizontal position {landingPosition.X} is invalid, must be in range [{minWidth}..{maxWidth}]");
        }

        const int minHeight = 0;
        var maxHeight = Map.Height;

        if (!Enumerable.Range(minHeight, maxHeight).Contains(landingPosition.Y))
        {
            throw new InvalidOperationException(
                $"landing vertical position {landingPosition.Y} is invalid, must be in range [{minHeight}..{maxHeight}]");
        }

        if (Map.HasObstacleAt(landingPosition))
        {
            throw new InvalidOperationException($"landing position {landingPosition} is invalid, Mars has an obstacle there!");
        }
    }


    public string Draw(int cellWidth = DomainConstants.MapDrawing.DefaultCellWidth, int cellHeight = DomainConstants.MapDrawing.CellHeight)
    {
        if (cellWidth < DomainConstants.MapDrawing.MinCellWidth)
        {
            throw new ArgumentException($"{nameof(cellWidth)} must be greater than {DomainConstants.MapDrawing.MinCellWidth}");
        }

        // adjust for cell margins
        var cellWidthWithMargin = cellWidth + 1;
        var cellHeightWithMargin = cellHeight + 1;

        var mapRepresentation = new string[Map.Height * cellHeightWithMargin + 1, Map.Width * cellWidthWithMargin + 1];

        // fill obstacle cells
        foreach (var obstacle in Map.Obstacles)
        {
            for (var dy = 0; dy < cellHeightWithMargin; dy++)
            {
                for (var dx = 0; dx < cellWidthWithMargin; dx++)
                {
                    mapRepresentation[obstacle.Y * cellHeightWithMargin + dy, obstacle.X * cellWidthWithMargin + dx] = "#";
                }
            }
        }

        // draw margins
        for (var y = 0; y < mapRepresentation.GetLength(0); y += cellHeightWithMargin)
        {
            for (var x = 0; x < mapRepresentation.GetLength(1); x++)
            {
                mapRepresentation[y, x] = "-";
            }
        }

        for (var y = 0; y < mapRepresentation.GetLength(0); y++)
        {
            for (var x = 0; x < mapRepresentation.GetLength(1); x += cellWidthWithMargin)
            {
                mapRepresentation[y, x] = "|";
            }
        }

        // draw rover if provided
        if (RoverState != null)
        {
            var roverX = RoverState.Position.X * cellWidthWithMargin + (int)Math.Ceiling((decimal)cellWidth / 2);
            var roverY = RoverState.Position.Y * cellHeightWithMargin + (int)Math.Ceiling((decimal)cellHeight / 2);

            mapRepresentation[roverY, roverX] = RoverState.Direction.AsMapDrawSymbol();
        }

        // convert representation
        var sb = new StringBuilder();
        if (RoverState == null)
        {
            sb.AppendLine("rover is in orbit!");
        }
        else
        {
            sb.AppendLine($"rover is at {RoverState}");
        }

        sb.AppendLine($"here's Mars! ({Map.Width}x{Map.Height}):");

        for (var y = 0; y < mapRepresentation.GetLength(0); y++)
        {
            for (var x = 0; x < mapRepresentation.GetLength(1); x++)
            {
                var coordinateRepresentation = mapRepresentation[y, x];
                sb.Append(string.IsNullOrWhiteSpace(coordinateRepresentation) ? " " : coordinateRepresentation);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
