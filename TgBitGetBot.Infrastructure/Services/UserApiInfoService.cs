using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.DataAccess.Repos;
using TgBitGetBot.DataAccess.Repos.Interfaces;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.Infrastructure.Services;

public class UserApiInfoService : IUserApiInfoService
{
	private readonly IMapper _mapper;
	private readonly IUserApiInfoRepository _userApiInfoRepository;
	private readonly ILogger<TickerService> _logger;

	public UserApiInfoService(IUserApiInfoRepository userApiInfoRepository, IMapper mapper, ILogger<TickerService> logger)
	{
		_userApiInfoRepository = userApiInfoRepository;
		_mapper = mapper;
		_logger = logger;
	}
	public async ValueTask<bool> AddUserApiInfo(UserApiInfoDto userApiInfo)
	{
		if (await CheckIsUserApiInfoExists(userApiInfo.Token))
		{
			return false;
		}

		try
		{
			var result = await _userApiInfoRepository.AddAsync(_mapper.Map<UserApiInfo>(userApiInfo));

			_logger.Log(LogLevel.Information,
				"Created {entity} with Id: {id}", nameof(result.Entity), result.Entity.Id);

			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError("Error: {message}\nStack trace: {stackTrace}", ex.Message, ex.StackTrace);
			return false;
		}
	}

	public async ValueTask<bool> CheckIsUserApiInfoExists(string token)
	{
		if (String.IsNullOrWhiteSpace(token))
		{
			throw new ArgumentNullException(nameof(token));
		}

		return (await _userApiInfoRepository.GetByConditionAsync(x => x.Token == token)).Any();
	}

	public async Task<List<UserApiInfoDto>> GetAllUserApiInfos(long userId)
	{
		var entities = await _userApiInfoRepository.GetByConditionAsync(x => x.User.TelegramId == userId);

		return _mapper.Map<List<UserApiInfoDto>>(entities);
	}

	public async ValueTask<bool> RemoveUserApiInfoByToken(string token)
	{
		if (await CheckIsUserApiInfoExists(token))
		{
			return false;
		}

		try
		{
			var apiInfoId = (await _userApiInfoRepository.GetByConditionAsync(x => x.Token == token)).FirstOrDefault().Id;

			var result = await _userApiInfoRepository.DeleteByIdAsync(apiInfoId);

			_logger.Log(LogLevel.Information,
				"Deleted {entity} with Id: {id}", nameof(result.Entity), result.Entity.Id);

			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError("Error: {message}\nStack trace: {stackTrace}", ex.Message, ex.StackTrace);
			return false;
		}
	}

	public ValueTask<bool> UpdateUserApiInfo(UserApiInfoDto userApiInfo)
	{
		throw new NotImplementedException();
	}
}