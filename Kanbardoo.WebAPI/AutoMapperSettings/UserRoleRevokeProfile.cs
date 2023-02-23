using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class UserRoleRevokeProfile : Profile
{
    public UserRoleRevokeProfile()
    {
        CreateMap<UserRoleRevokeDTO, UserRoleRevokeModel>();
    }
}