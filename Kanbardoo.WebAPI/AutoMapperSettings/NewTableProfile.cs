using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class NewTableProfile : Profile
{
	public NewTableProfile()
	{
		CreateMap<NewTableDTO, NewTable>();
	}
}
