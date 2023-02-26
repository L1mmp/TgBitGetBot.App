using TgBitGetBot.Domain.Dtos;

namespace TgBitGetBot.Infrastructure.Services.Interfaces;

public interface IUserService
{
	public Task<bool> CheckIfUserExists();
	public Task<bool> AddUser(UserDto userDto);
	public Task RemoveUserById(long Id);

}