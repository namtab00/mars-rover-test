using MarsRover.Domain.Mission;

namespace MarsRover.Domain.Entities;

public class Rover(RoverState state)
{
    private Dictionary<char, Func<MarsMap, RoverState>> CommandMap =>
        new() {
            { 'f', Proceed },
            { 'b', Reverse },
            { 'l', TurnLeft },
            { 'r', TurnRight },
            { 'j', Jump },
        };


    public CommandBatchResult ProcessCommands(List<char> commands, MarsMap map)
    {
        var commandResults = new List<CommandResult>();

        var abortPending = false;
        MapCoordinate? foundObstacle = null;
        foreach (var (command, index) in commands.Select((command, index) => (command, index)))
        {
            if (abortPending)
            {
                commandResults.Add(new CommandResult(index, command, CommandOutcomeType.AbortedDueToObstacle, AttemptedToMoveAt: null));
                continue;
            }

            var commandResult = ProcessCommand(index, command, map);
            if (commandResult.Outcome == CommandOutcomeType.AbortedDueToObstacle)
            {
                foundObstacle = commandResult.AttemptedToMoveAt!;
                abortPending = true;
            }

            commandResults.Add(commandResult);
        }

        return new CommandBatchResult(MissionStatus: new MissionStatus(map, state), CommandResults: commandResults,
            EncounteredObstacleAt: foundObstacle);
    }


    private CommandResult ProcessCommand(int index, char command, MarsMap map)
    {
        if (!CommandMap.TryGetValue(command, out var commandProcessor))
        {
            return new CommandResult(index, command, CommandOutcomeType.Unknown, AttemptedToMoveAt: null);
        }

        var candidateState = commandProcessor.Invoke(map);

        if (map.HasObstacleAt(candidateState.Position))
        {
            return new CommandResult(index, command, CommandOutcomeType.AbortedDueToObstacle, candidateState.Position);
        }

        state = candidateState;

        return new CommandResult(index, command, CommandOutcomeType.Processed, candidateState.Position);
    }


    private RoverState Jump(MarsMap map)
    {
        var newCandidateState = Move(state, map, forward: true);
        newCandidateState = Move(newCandidateState, map, forward: true);

        return newCandidateState;
    }


    private RoverState Proceed(MarsMap map)
    {
        return Move(state, map, forward: true);
    }


    private RoverState Reverse(MarsMap map)
    {
        return Move(state, map, forward: false);
    }


    private static RoverState Move(RoverState currentState, MarsMap map, bool forward)
    {
        var newX = currentState.Position.X;
        var newY = currentState.Position.Y;

        var xOffset = 0;
        var yOffset = 0;

        // Determined offsets
        switch (currentState.Direction)
        {
            case Direction.North:
                yOffset = forward ? -1 : 1;
                break;
            case Direction.East:
                xOffset = forward ? 1 : -1;
                break;
            case Direction.South:
                yOffset = forward ? 1 : -1;
                break;
            case Direction.West:
                xOffset = forward ? -1 : 1;
                break;
            default:
                throw new InvalidOperationException($"Invalid direction {currentState.Direction:G}");
        }

        // Calculate new coords with wrapping
        newX = (newX + xOffset + map.Width) % map.Width;
        newY = (newY + yOffset + map.Height) % map.Height;

        return currentState with { Position = new MapCoordinate { X = newX, Y = newY } };
    }


    private RoverState TurnLeft(MarsMap map)
    {
        var newDirection = state.Direction switch {
            Direction.North => Direction.West,
            Direction.East => Direction.North,
            Direction.South => Direction.East,
            Direction.West => Direction.South,
            _ => throw new InvalidOperationException("Invalid direction")
        };
        return state with { Direction = newDirection };
    }


    private RoverState TurnRight(MarsMap map)
    {
        var newDirection = state.Direction switch {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new InvalidOperationException("Invalid direction")
        };

        return state with { Direction = newDirection };
    }
}
