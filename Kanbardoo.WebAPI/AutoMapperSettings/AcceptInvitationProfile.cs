using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class AcceptInvitationProfile : Profile
{
    public AcceptInvitationProfile()
    {
        CreateMap<AcceptInvitationDTO, AcceptInvitation>();
    }
}