using TgBitGetBot.Domain.Dtos;

namespace TgBitGetBot.Infrastructure.Services;

public interface IUserApiInfoService
{
	public Task<bool> AddUserApiInfo(UserApiInfoDto userApiInfo);
	public Task<bool> UpdateUserApiInfo(UserApiInfoDto userApiInfo);
	public Task<bool> CheckIsUserApiInfoExists(string token);
	public Task RemoveUserApiInfoByToken(string token);
	public Task<List<UserApiInfoDto>> GetAllUserApiInfos(long userId);
}