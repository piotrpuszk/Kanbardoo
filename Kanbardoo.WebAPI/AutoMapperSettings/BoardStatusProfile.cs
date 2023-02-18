using AutoMapper;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class BoardStatusProfile : Profile
{
    public BoardStatusProfile()
    {
        CreateMap<KanBoardStatus, KanBoardStatusDTO>().ReverseMap();
    }
}
