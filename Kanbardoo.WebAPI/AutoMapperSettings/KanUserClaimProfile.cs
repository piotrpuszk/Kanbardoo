using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class KanUserClaimProfile : Profile
{
    public KanUserClaimProfile()
    {
        CreateMap<KanUserClaimDTO, KanUserClaimModel>();
    }
}
