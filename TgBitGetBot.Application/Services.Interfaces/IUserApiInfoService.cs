using TgBitGetBot.Domain.Dtos;

namespace TgBitGetBot.Application.Services.Interfaces;

public interface IUserApiInfoService
{
	public ValueTask<bool> AddUserApiInfo(UserApiInfoDto userApiInfo);
	public ValueTask<bool> UpdateUserApiInfo(UserApiInfoDto userApiInfo);
	public ValueTask<bool> CheckIsUserApiInfoExists(string token);
	public ValueTask<bool> RemoveUserApiInfoByToken(string token);
	public Task<List<UserApiInfoDto>> GetAllUserApiInfos(long userId);
}