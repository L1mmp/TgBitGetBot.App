using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.Application.Services.Interfaces;

public interface IUserService
{
	public ValueTask<bool> AddUser(UserDto userDto);
	public ValueTask<bool> RemoveUserById(long Id);

}