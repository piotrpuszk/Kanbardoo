using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class KanBoardUserProfile : Profile
{
	public KanBoardUserProfile()
	{
		CreateMap<KanBoardUser, KanBoardUserDTO>();
	}
}
