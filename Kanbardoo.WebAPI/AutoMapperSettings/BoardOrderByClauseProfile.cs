using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.WebAPI.FilterDTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class BoardOrderByClauseProfile : Profile
{
	public BoardOrderByClauseProfile()
	{
		CreateMap<OrderByClauseDTO, OrderByClause<KanBoard>>();
	}
}
