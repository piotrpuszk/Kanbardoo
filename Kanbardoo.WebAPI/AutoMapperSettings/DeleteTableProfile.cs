using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class DeleteTableProfile : Profile
{
	public DeleteTableProfile()
	{
		CreateMap<DeleteTableDTO, DeleteTable>();
	}
}

public class GetTableProfile : Profile
{
	public GetTableProfile()
	{
		//CreateMap<GetTableDTO, GetTable>();
	}
}