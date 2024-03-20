using AutoMapper;
using MarsRover.API.Startup;

namespace MarsRover.API.Tests;

public class MappingProfilesShould
{
    private readonly IMapper _sut;

    public MappingProfilesShould()
    {
        var assembly = typeof(Program).Assembly;

        var config = new MapperConfiguration(opt => opt.AddMaps(assembly));

        _sut = config.CreateMapper();
    }

    [Fact]
    public void HaveAValidConfiguration() => _sut.ConfigurationProvider.AssertConfigurationIsValid();
}