using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.Domain.MappingProfiles
{
	public class DtoToEntitesProfile : Profile
	{
		public DtoToEntitesProfile()
		{
			CreateMap<User, UserDto>().ReverseMap();
		}
	}
}
