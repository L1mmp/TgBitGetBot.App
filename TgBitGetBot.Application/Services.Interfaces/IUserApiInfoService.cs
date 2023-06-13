using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.Application.Services.Interfaces;

public interface IUserApiInfoService
{
	public Task<bool> AddUserApiInfo(UserApiInfoDto userApiInfo);
	public Task<bool> UpdateUserApiInfo(UserApiInfoDto userApiInfo);
	public Task<bool> UpdateUserApiInfo(UserApiInfo userApiInfo);
	public ValueTask<bool> CheckIsUserApiInfoExists(string token);
	public ValueTask<bool> RemoveUserApiInfoByToken(string token);
	public Task<List<UserApiInfoDto>> GetAllUserApiInfos(long userId);
	public Task<UserApiInfoDto> GetUserApiInfoByToken(string token);
	public Task<UserApiInfo> GetLatestUserApiInfo(long userId);
	public Task<UserApiInfo> GetUserApiInfoByUserTelegramId(long userId);
}