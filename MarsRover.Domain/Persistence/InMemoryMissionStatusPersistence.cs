using MarsRover.Domain.Entities;
using MarsRover.Domain.Mission;

namespace MarsRover.Domain.Persistence;

public class InMemoryMissionStatusPersistence : IMissionStatusPersistenceProvider
{
    private static MarsMap _map = MarsMap.Generate(DomainConstants.DefaultMap.Width, DomainConstants.DefaultMap.Height,
        DomainConstants.DefaultMap.ObstacleCount);

    private static RoverState? _roverState;


    public async Task<MissionStatus> GetStatusAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(new MissionStatus(_map, _roverState));
    }


    public async Task<MissionStatus> ResetStatusAsync(CancellationToken ct = default)
    {
        var newMap = MarsMap.Generate(DomainConstants.DefaultMap.Width, DomainConstants.DefaultMap.Height, DomainConstants.DefaultMap.ObstacleCount);
        var newStatus = new MissionStatus(newMap, RoverState: null);
        await SaveStatusAsync(newStatus, ct);

        return await Task.FromResult(newStatus);
    }


    public async Task SaveStatusAsync(MissionStatus status, CancellationToken ct = default)
    {
        _map = status.Map;
        _roverState = status.RoverState;

        await Task.CompletedTask;
    }
}
