using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class SignInProfile : Profile
{
	public SignInProfile()
	{
		CreateMap<SignInDTO, SignIn>();
	}
}
