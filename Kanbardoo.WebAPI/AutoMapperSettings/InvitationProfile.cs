using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class InvitationProfile : Profile
{
    public InvitationProfile()
    {
        CreateMap<InvitationDTO, Invitation>().ReverseMap();
    }
}