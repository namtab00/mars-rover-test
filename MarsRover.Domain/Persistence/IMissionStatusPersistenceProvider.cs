using MarsRover.Domain.Mission;

namespace MarsRover.Domain.Persistence;

public interface IMissionStatusPersistenceProvider
{
    Task<MissionStatus> GetStatusAsync(CancellationToken ct = default);


    Task<MissionStatus> ResetStatusAsync(CancellationToken ct = default);


    Task SaveStatusAsync(MissionStatus status, CancellationToken ct = default);
}
