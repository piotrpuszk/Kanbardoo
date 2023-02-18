using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class KanUserClaimProfile : Profile
{
	public KanUserClaimProfile()
	{
		CreateMap<KanUserClaimDTO, KanUserClaim>();
	}
}
