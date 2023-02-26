using AutoMapper;
using TgBitGetBot.DataAccess.Repos;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Infrastructure.Services.Interfaces;

namespace TgBitGetBot.Infrastructure.Services;

public class UserService : IUserService
{
	private IMapper _mapper;
	private UserRepository _userRepository;
	public Task<bool> AddUser(UserDto userDto)
	{
		throw new NotImplementedException();
	}

	public Task RemoveUserById(long Id)
	{
		throw new NotImplementedException();
	}

	Task<bool> IUserService.CheckIfUserExists()
	{
		throw new NotImplementedException();
	}
}