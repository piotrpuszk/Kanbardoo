using AutoMapper;
using Kanbardoo.WebAPI.DTOs;
using KanTask = Kanbardoo.Domain.Entities.KanTask;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class TaskProfile : Profile
{
	public TaskProfile()
	{
		CreateMap<KanTask, KanTaskDTO>().ReverseMap();
	}
}
