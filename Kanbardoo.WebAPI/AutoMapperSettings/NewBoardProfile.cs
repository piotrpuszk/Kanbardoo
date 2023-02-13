using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class NewBoardProfile : Profile
{
	public NewBoardProfile()
	{
		CreateMap<NewBoard, NewBoardDTO>().ReverseMap();
	}
}
