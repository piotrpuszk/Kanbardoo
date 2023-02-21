using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class DeleteUserClaimProfile : Profile
{
    public DeleteUserClaimProfile()
    {
        CreateMap<DeleteUserClaimDTO, KanUserClaimModel>();
    }
}