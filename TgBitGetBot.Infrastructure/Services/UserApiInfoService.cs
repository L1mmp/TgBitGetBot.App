using TgBitGetBot.Domain.Dtos;

namespace TgBitGetBot.Infrastructure.Services;

public class UserApiInfoService : IUserApiInfoService
{
	public bool AddUserApiInfo(UserApiInfoDto userApiInfo)
	{
		throw new NotImplementedException();
	}

	Task<bool> IUserApiInfoService.UpdateUserApiInfo(UserApiInfoDto userApiInfo)
	{
		throw new NotImplementedException();
	}

	Task<bool> IUserApiInfoService.CheckIsUserApiInfoExists(string token)
	{
		throw new NotImplementedException();
	}

	public Task RemoveUserApiInfoByToken(string token)
	{
		throw new NotImplementedException();
	}

	Task<List<UserApiInfoDto>> IUserApiInfoService.GetAllUserApiInfos(long userId)
	{
		throw new NotImplementedException();
	}

	Task<bool> IUserApiInfoService.AddUserApiInfo(UserApiInfoDto userApiInfo)
	{
		throw new NotImplementedException();
	}
}