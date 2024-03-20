using MarsRover.Domain.Mission;

namespace MarsRover.API.Controllers.Mission.Dtos;

public class CommandResultDto

{
    public int CommandIndex { get; set; }

    public char IssuedCommand { get; set; }

    public CommandOutcomeType Outcome { get; set; }
}