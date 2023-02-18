using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class KanRoleClaimProfile : Profile
{
    public KanRoleClaimProfile()
    {
        CreateMap<KanRoleClaimDTO, KanRoleClaim>();
    }
}
