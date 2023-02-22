using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class NewInvitationProfile : Profile
{
    public NewInvitationProfile()
    {
        CreateMap<NewInvitationDTO, NewInvitation>();
    }
}
