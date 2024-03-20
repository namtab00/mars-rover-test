using AutoMapper;
using MarsRover.Domain.Entities;
using MarsRover.Domain.Mission;

namespace MarsRover.API.Controllers.Mission.Dtos;

public class MissionMappingProfile : Profile
{
    public MissionMappingProfile()
    {
        CreateMap<MapCoordinate, MapCoordinateResultDto>();

        CreateMap<RoverState, RoverStateResultDto>();

        CreateMap<MissionStatus, MissionStatusResultDto>()
            .ForMember(dto => dto.TotalKnownObstacleCount, opt => opt.MapFrom(src => src.Map.Obstacles.Count));

        CreateMap<LandingRequestDto, RoverState>()
            .ForCtorParam(nameof(RoverState.Position),
                opt => opt.MapFrom(src => new MapCoordinate { X = src.X, Y = src.Y }))
            .ForCtorParam(nameof(RoverState.Direction), opt => opt.MapFrom(src => src.Direction));

        CreateMap<CommandResult, CommandResultDto>();

        CreateMap<CommandBatchResult, CommandBatchResultDto>()
            .ForMember(dto => dto.RoverState, opt => opt.MapFrom(src => src.MissionStatus.RoverState));
    }
}