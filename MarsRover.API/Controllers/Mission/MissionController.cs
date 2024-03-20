using AutoMapper;
using MarsRover.API.Controllers.Mission.Dtos;
using MarsRover.Domain.Entities;
using MarsRover.Domain.Mission;
using Microsoft.AspNetCore.Mvc;

namespace MarsRover.API.Controllers.Mission;

[ApiController]
[Route("mission")]
public class MissionController(IMissionControl missionControl, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("status")]
    public async Task<MissionStatusResultDto> GetStatusAsync(CancellationToken ct = default)
    {
        var missionStatus = await missionControl.GetStatusAsync(ct);

        var result = mapper.Map<MissionStatus, MissionStatusResultDto>(missionStatus);
        return result;
    }


    [HttpPost]
    [Route("land/at/random")]
    public async Task<MissionStatusResultDto> LandAsync(CancellationToken ct = default)
    {
        var missionStatus = await missionControl.LandAsync(initialRoverState: null, ct);

        var result = mapper.Map<MissionStatus, MissionStatusResultDto>(missionStatus);
        return result;
    }


    [HttpPost]
    [Route("land/at")]
    public async Task<MissionStatusResultDto> LandAsync(LandingRequestDto request, CancellationToken ct = default)
    {
        var initialRoverState = mapper.Map<LandingRequestDto, RoverState>(request);

        var missionStatus = await missionControl.LandAsync(initialRoverState, ct);

        var result = mapper.Map<MissionStatus, MissionStatusResultDto>(missionStatus);
        return result;
    }


    [HttpPost]
    [Route("abort")]
    public async Task<MissionStatusResultDto> AbortAsync(CancellationToken ct = default)
    {
        var missionStatus = await missionControl.AbortAsync(ct);
        var result = mapper.Map<MissionStatus, MissionStatusResultDto>(missionStatus);
        return result;
    }


    [HttpPost]
    [Route("process")]
    public async Task<CommandBatchResultDto> ProcessCommandsAsync(List<char> commands, CancellationToken ct = default)
    {
        var processingResult = await missionControl.SendCommandsAsync(commands, ct);

        var result = mapper.Map<CommandBatchResult, CommandBatchResultDto>(processingResult);
        return result;
    }
}
