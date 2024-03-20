using MarsRover.Domain.Entities;

namespace MarsRover.Domain.Mission;

public interface IMissionControl
{
    /// <summary>
    ///     Lands the rover at a random location
    /// </summary>
    Task<MissionStatus> LandAsync(RoverState? initialRoverState, CancellationToken ct = default);


    /// <summary>
    ///     Aborts the mission (returns lander to orbit)
    /// </summary>
    Task<MissionStatus> AbortAsync(CancellationToken ct = default);


    /// <summary>
    ///     Returns mission status
    /// </summary>
    Task<MissionStatus> GetStatusAsync(CancellationToken ct = default);


    /// <summary>
    ///     Sends rover commands for processing
    /// </summary>
    Task<CommandBatchResult> SendCommandsAsync(List<char> commands, CancellationToken ct = default);
}
