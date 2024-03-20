using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace MarsRover.Domain.Tests;

[SuppressMessage("ReSharper", "CommentTypo")]
public class RoverTestsBase(ITestOutputHelper outputHelper)
{
    protected ITestOutputHelper OutputHelper => outputHelper;
}