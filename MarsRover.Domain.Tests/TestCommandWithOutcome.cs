using MarsRover.Domain.Mission;
using Xunit.Abstractions;

namespace MarsRover.Domain.Tests;

public class TestCommandWithOutcome : IXunitSerializable
{
    public TestCommandWithOutcome() {}


    public TestCommandWithOutcome(char command, CommandOutcomeType expectedOutcome)
    {
        Command = command;
        ExpectedOutcome = expectedOutcome;
    }


    public char Command { get; set; }

    public CommandOutcomeType ExpectedOutcome { get; set; }


    public void Deserialize(IXunitSerializationInfo info)
    {
        Command = info.GetValue<char>(nameof(Command));
        ExpectedOutcome = info.GetValue<CommandOutcomeType>(nameof(ExpectedOutcome));
    }


    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(Command), Command);
        info.AddValue(nameof(ExpectedOutcome), ExpectedOutcome);
    }
}
