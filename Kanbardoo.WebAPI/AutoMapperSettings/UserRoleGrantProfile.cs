using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class UserBoardRoleGrantProfile : Profile
{
    public UserBoardRoleGrantProfile()
    {
        CreateMap<UserBoardRoleGrantDTO, UserBoardRoleGrantModel>();
    }
}
