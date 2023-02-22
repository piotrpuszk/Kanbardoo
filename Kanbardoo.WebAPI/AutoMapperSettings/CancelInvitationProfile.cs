using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class CancelInvitationProfile : Profile
{
    public CancelInvitationProfile()
    {
        CreateMap<CancelInvitationDTO, CancelInvitationModel>();
    }
}
