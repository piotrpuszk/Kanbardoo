using AutoMapper;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;

namespace Kanbardoo.WebAPI.AutoMapperSettings;

public class SignUpProfile : Profile
{
	public SignUpProfile()
	{
		CreateMap<SignUpDTO, SignUp>();
	}
}
