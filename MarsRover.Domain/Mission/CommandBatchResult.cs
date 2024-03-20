using MarsRover.Domain.Entities;

namespace MarsRover.Domain.Mission;

public record CommandBatchResult(MissionStatus MissionStatus, List<CommandResult> CommandResults, MapCoordinate? EncounteredObstacleAt);
