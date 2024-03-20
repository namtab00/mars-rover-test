using MarsRover.API.Controllers.Mission.Dtos;
using MarsRover.Domain.Mission;
using MarsRover.Domain.Persistence;

namespace MarsRover.API.Startup;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers Mars rover services
    /// </summary>
    public static IServiceCollection AddRoverServices(this IServiceCollection services)
    {
        services.AddTransient<IMissionControl, MissionControl>();

        services.AddScoped<IMissionStatusPersistenceProvider, InMemoryMissionStatusPersistence>();

        services.AddAutoMapper(typeof(MissionMappingProfile));

        return services;
    }
}