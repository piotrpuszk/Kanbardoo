using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class TableProfile : Profile
{
	public TableProfile()
	{
		CreateMap<Table, TableDTO>().ReverseMap();
	}
}
