using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class KanUserRoleProfile : Profile
{
    public KanUserRoleProfile()
    {
        CreateMap<KanUserRoleDTO, KanUserBoardRole>();
    }
}
