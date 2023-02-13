using AutoMapper;
using Kanbardoo.WebAPI.DTOs;
using TaskStatus = Kanbardoo.Domain.Entities.TaskStatus;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class TaskStatusProfile : Profile
{
    public TaskStatusProfile()
    {
        CreateMap<TaskStatus, TaskStatusDTO>().ReverseMap();
    }
}
