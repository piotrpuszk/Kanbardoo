using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class NewTaskProfile : Profile
{
	public NewTaskProfile()
	{
		CreateMap<NewKanTaskDTO, NewKanTask>()
			.ForPath(e => e.StatusID, e => e.MapFrom(x => x.Status.ID))
			.ForPath(e => e.AssigneeID, e => e.MapFrom(x => x.Assignee.ID));
	}
}
