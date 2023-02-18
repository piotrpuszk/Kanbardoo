using AutoMapper;
using Kanbardoo.Domain.Filters;
using Kanbardoo.WebAPI.FilterDTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class BoardFiltersProfile : Profile
{
	public BoardFiltersProfile()
	{
		CreateMap<KanBoardFiltersDTO, KanBoardFilters>();
	}
}
