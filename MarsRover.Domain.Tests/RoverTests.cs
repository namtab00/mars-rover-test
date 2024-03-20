using MarsRover.Domain.Entities;
using MarsRover.Domain.Mission;
using Shouldly;
using Xunit.Abstractions;

namespace MarsRover.Domain.Tests;

public sealed class RoverTests(ITestOutputHelper outputHelper) : RoverTestsBase(outputHelper)
{
    /*
    |  0  |  1  |  2  |  3  |  4  |
----|-----|-----|-----|-----|-----|
    |     |     |     |#####|     |
  0 |     |     |     |#####|     |
    |     |     |     |#####|     |
----|-----|-----|-----|-----|-----|
    |#####|     |     |     |     |
  1 |#####|     |     |     |     |
    |#####|     |     |     |     |
----|-----|-----|-----|-----|-----|
    |     |     |#####|     |     |
  2 |     |     |#####|     |     |
    |     |     |#####|     |     |
----|-----|-----|-----|-----|-----|
    |     |     |     |     |#####|
  3 |     |     |     |     |#####|
    |     |     |     |     |#####|
----|-----|-----|-----|-----|-----|
    |     |#####|     |     |     |
  4 |     |#####|     |     |     |
    |     |#####|     |     |     |
----|-----|-----|-----|-----|-----|
     */
    private readonly MarsMap _testMapStub = new(5, 5, [
        new MapCoordinate { X = 3, Y = 0 },
        new MapCoordinate { X = 0, Y = 1 },
        new MapCoordinate { X = 2, Y = 2 },
        new MapCoordinate { X = 4, Y = 3 },
        new MapCoordinate { X = 1, Y = 4 }
    ]);


    public static readonly TheoryData<List<TestCommandWithOutcome>>
        TestCommandsWithUnknownOnes =
            new()
            {
                new List<TestCommandWithOutcome>
                {
                    new('a', CommandOutcomeType.Unknown),
                    new(' ', CommandOutcomeType.Unknown),
                    new('_', CommandOutcomeType.Unknown),
                    new(':', CommandOutcomeType.Unknown)
                },
                new List<TestCommandWithOutcome>
                {
                    new('x', CommandOutcomeType.Unknown),
                    new('l', CommandOutcomeType.Processed),
                    new('c', CommandOutcomeType.Unknown),
                    new('f', CommandOutcomeType.Processed),
                    new('!', CommandOutcomeType.Unknown),
                    new('w', CommandOutcomeType.Unknown),
                    new('b', CommandOutcomeType.Processed)
                }
            };


    [Theory]
    [MemberData(nameof(TestCommandsWithUnknownOnes))]
    public void Rover_IgnoresUnknownCommands(
        List<TestCommandWithOutcome> commandsWithOutcome)
    {
        // Arrange
        var startPosition = new MapCoordinate { X = 0, Y = 0 };
        var missionStatus = new MissionStatus(_testMapStub, new RoverState(startPosition, Direction.South));
        OutputHelper.WriteLine(missionStatus.Draw());

        var commandsToProcess = commandsWithOutcome.Select(t => t.Command).ToList();

        var sut = new Rover(missionStatus.RoverState!);

        // Act
        var result = sut.ProcessCommands(commandsToProcess, missionStatus.Map);
        OutputHelper.WriteLine(result.MissionStatus.Draw());

        // Assert
        result.CommandResults.Count.ShouldBe(commandsToProcess.Count);
        foreach (var (commandIndex, _, expectedOutcome) in commandsWithOutcome.Select((commandWithOutcome, index) =>
                     (index, commandWithOutcome.Command, commandWithOutcome.ExpectedOutcome)))
        {
            var commandResult = result.CommandResults.Single(r => r.CommandIndex == commandIndex);
            commandResult.Outcome.ShouldBe(expectedOutcome);
        }
    }

    public static readonly TheoryData<List<TestCommandWithOutcome>>
        TestCommandsHittingObstacles =
            new()
            {
                new List<TestCommandWithOutcome>
                {
                    new('f', CommandOutcomeType.AbortedDueToObstacle),
                    new('f', CommandOutcomeType.AbortedDueToObstacle),
                    new('l', CommandOutcomeType.AbortedDueToObstacle),
                    new('f', CommandOutcomeType.AbortedDueToObstacle)
                },
                new List<TestCommandWithOutcome>
                {
                    new('l', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.AbortedDueToObstacle),
                    new('r', CommandOutcomeType.AbortedDueToObstacle)
                },
                new List<TestCommandWithOutcome>
                {
                    new('l', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.AbortedDueToObstacle)
                },
                new List<TestCommandWithOutcome>
                {
                    new('r', CommandOutcomeType.Processed),
                    new('b', CommandOutcomeType.Processed),
                    new('b', CommandOutcomeType.Processed),
                    new('b', CommandOutcomeType.AbortedDueToObstacle)
                },
                new List<TestCommandWithOutcome>
                {
                    new('l', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.Processed),
                    new('r', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.Processed),
                    new('f', CommandOutcomeType.AbortedDueToObstacle)
                }
            };

    [Theory]
    [MemberData(nameof(TestCommandsHittingObstacles))]
    public void Rover_HaltsAtObstacle(List<TestCommandWithOutcome> commandsWithOutcome)
    {
        // Arrange
        var startPosition = new MapCoordinate { X = 0, Y = 0 };
        var missionStatus = new MissionStatus(_testMapStub, new RoverState(startPosition, Direction.South));
        OutputHelper.WriteLine(missionStatus.Draw());

        var commandsToProcess = commandsWithOutcome.Select(t => t.Command).ToList();

        var sut = new Rover(missionStatus.RoverState!);

        // Act
        var result = sut.ProcessCommands(commandsToProcess, missionStatus.Map);
        OutputHelper.WriteLine(result.MissionStatus.Draw());

        // Assert
        result.CommandResults.Count.ShouldBe(commandsWithOutcome.Count);
        foreach (var (commandIndex, _, expectedOutcome) in commandsWithOutcome.Select((commandWithOutcome, index) =>
                     (index, commandWithOutcome.Command, commandWithOutcome.ExpectedOutcome)))
        {
            var commandResult = result.CommandResults.Single(r => r.CommandIndex == commandIndex);
            commandResult.Outcome.ShouldBe(expectedOutcome);
        }
        result.EncounteredObstacleAt.ShouldNotBeNull();
    }

    public static readonly TheoryData<List<TestCommandWithOutcome>>
        TestCommandsForObstacleJumping =
            new()
            {
                new List<TestCommandWithOutcome>
                {
                    new('j', CommandOutcomeType.Processed)
                }
            };

    [Theory]
    [MemberData(nameof(TestCommandsForObstacleJumping))]
    public void Rover_JumpsObstacle(List<TestCommandWithOutcome> commandsWithOutcome)
    {
        // Arrange
        var startPosition = new MapCoordinate { X = 0, Y = 0 };
        var missionStatus = new MissionStatus(_testMapStub, new RoverState(startPosition, Direction.South));
        OutputHelper.WriteLine(missionStatus.Draw());

        var commandsToProcess = commandsWithOutcome.Select(t => t.Command).ToList();

        var sut = new Rover(missionStatus.RoverState!);

        // Act
        var result = sut.ProcessCommands(commandsToProcess, missionStatus.Map);
        OutputHelper.WriteLine(result.MissionStatus.Draw());

        // Assert
        result.CommandResults.Count.ShouldBe(commandsWithOutcome.Count);
        foreach (var (commandIndex, _, expectedOutcome) in commandsWithOutcome.Select((commandWithOutcome, index) =>
                     (index, commandWithOutcome.Command, commandWithOutcome.ExpectedOutcome)))
        {
            var commandResult = result.CommandResults.Single(r => r.CommandIndex == commandIndex);
            commandResult.Outcome.ShouldBe(expectedOutcome);
        }
        result.EncounteredObstacleAt.ShouldBeNull();
    }
}
