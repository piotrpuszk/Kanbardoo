using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class TaskProfile : Profile
{
	public TaskProfile()
	{
		CreateMap<KanTask, KanTaskDTO>().ReverseMap();
	}
}
