using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class DeclineInvitationProfile : Profile
{
	public DeclineInvitationProfile()
	{
		CreateMap<DeclineInvitationDTO, DeclineInvitation>();
	}
}
