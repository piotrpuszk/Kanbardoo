using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class TaskStatusProfile : Profile
{
    public TaskStatusProfile()
    {
        CreateMap<KanTaskStatus, KanTaskStatusDTO>().ReverseMap();
    }
}
