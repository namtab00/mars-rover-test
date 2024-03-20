namespace MarsRover.Domain.Entities;

public record MarsMap(int Width, int Height, HashSet<MapCoordinate> Obstacles)
{
    public static MarsMap Generate(int width, int height, int obstacleCount)
    {
        var maxObstacleCount = Math.Floor((decimal)width * height / 2);
        if (obstacleCount > maxObstacleCount)
        {
            throw new InvalidOperationException($"{nameof(obstacleCount)} cannot exceed {maxObstacleCount}");
        }

        var obstacles = GenerateObstacles(width, height, obstacleCount);
        return new MarsMap(width, height, obstacles);
    }


    public bool HasObstacleAt(MapCoordinate point)
    {
        return Obstacles.Contains(point);
    }


    public RoverState PickRandomRoverState()
    {
        var random = new Random();
        var candidateLandingSite = new MapCoordinate { X = random.Next(Width), Y = random.Next(Height) };
        while (Obstacles.Contains(candidateLandingSite))
        {
            candidateLandingSite = new MapCoordinate { X = random.Next(Width), Y = random.Next(Height) };
        }

        return new RoverState(candidateLandingSite, Direction.North);
    }


    private static HashSet<MapCoordinate> GenerateObstacles(int width, int height, int obstacleCount)
    {
        var random = new Random();
        var generatedObstacles = new HashSet<MapCoordinate>();

        while (generatedObstacles.Count < obstacleCount)
        {
            var candidateObstacle = new MapCoordinate { X = random.Next(width), Y = random.Next(height) };
            while (generatedObstacles.Contains(candidateObstacle))
            {
                candidateObstacle = new MapCoordinate { X = random.Next(width), Y = random.Next(height) };
            }

            generatedObstacles.Add(candidateObstacle);
        }

        return generatedObstacles;
    }
}
