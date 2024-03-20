using MarsRover.Domain.Entities;

namespace MarsRover.Domain.Mission;

public record CommandResult(int CommandIndex, char IssuedCommand, CommandOutcomeType Outcome, MapCoordinate? AttemptedToMoveAt);
