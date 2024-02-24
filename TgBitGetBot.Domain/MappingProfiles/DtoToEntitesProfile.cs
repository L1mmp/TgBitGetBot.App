using AutoMapper;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.Domain.MappingProfiles
{
	public class DtoToEntitesProfile : Profile
	{
		public DtoToEntitesProfile()
		{
			CreateMap<User, UserDto>().ReverseMap();
			CreateMap<UserApiInfo, UserApiInfoDto>().ReverseMap();
		}
	}
}
