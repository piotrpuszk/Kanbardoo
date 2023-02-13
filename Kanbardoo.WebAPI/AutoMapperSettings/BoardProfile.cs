using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class BoardProfile : Profile
{
	public BoardProfile()
	{
		CreateMap<Board, BoardDTO>()
			.ForPath(e => e.Status.ID, e => e.MapFrom(e => e.StatusID))
			.ForPath(e => e.Owner.ID, e => e.MapFrom(e => e.OwnerID))
			.ForMember(e => e.Status, e => e.MapFrom(e => e.Status))
			.ForMember(e => e.Owner, e => e.MapFrom(e => e.Owner))
			.ReverseMap();
    }
}
