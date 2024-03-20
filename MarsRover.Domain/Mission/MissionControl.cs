using MarsRover.Domain.Entities;
using MarsRover.Domain.Persistence;
using Microsoft.Extensions.Logging;

namespace MarsRover.Domain.Mission;

public class MissionControl(ILoggerFactory loggerFactory, IMissionStatusPersistenceProvider persistenceProvider) : IMissionControl
{
    private readonly ILogger<MissionControl> _logger = loggerFactory.CreateLogger<MissionControl>();


    /// <inheritdoc />
    public async Task<MissionStatus> LandAsync(RoverState? initialRoverState, CancellationToken ct = default)
    {
        var missionStatus = await persistenceProvider.GetStatusAsync(ct);
        if (missionStatus.RoverState != null)
        {
            throw new InvalidOperationException($"rover is already at {missionStatus.RoverState}");
        }

        initialRoverState ??= missionStatus.Map.PickRandomRoverState();

        missionStatus.IsValidLandingPositionOrThrow(initialRoverState.Position);

        missionStatus = missionStatus with { RoverState = initialRoverState };

        await persistenceProvider.SaveStatusAsync(missionStatus, ct);

        _logger.LogDebug(missionStatus.Draw());

        return missionStatus;
    }


    /// <inheritdoc />
    public async Task<MissionStatus> AbortAsync(CancellationToken ct = default)
    {
        var newMissionStatus = await persistenceProvider.ResetStatusAsync(ct);

        await persistenceProvider.SaveStatusAsync(newMissionStatus, ct);

        _logger.LogDebug(newMissionStatus.Draw());

        return newMissionStatus;
    }


    /// <inheritdoc />
    public async Task<MissionStatus> GetStatusAsync(CancellationToken ct = default)
    {
        var missionStatus = await persistenceProvider.GetStatusAsync(ct);
        _logger.LogDebug(missionStatus.Draw());
        return missionStatus;
    }


    /// <inheritdoc />
    public async Task<CommandBatchResult> SendCommandsAsync(List<char> commands, CancellationToken ct = default)
    {
        var missionStatus = await persistenceProvider.GetStatusAsync(ct);
        if (missionStatus.RoverState == null)
        {
            throw new InvalidOperationException("rover is in orbit, must land first!");
        }

        var rover = new Rover(missionStatus.RoverState);
        var batchResult = rover.ProcessCommands(commands, missionStatus.Map);

        await persistenceProvider.SaveStatusAsync(batchResult.MissionStatus, ct);

        _logger.LogDebug(batchResult.MissionStatus.Draw());

        return batchResult;
    }
}
