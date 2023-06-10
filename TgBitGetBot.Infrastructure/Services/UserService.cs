using AutoMapper;
using Microsoft.Extensions.Logging;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.DataAccess.Repos.Interfaces;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.Infrastructure.Services;

public class UserService : IUserService
{
	private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;
	private readonly ILogger<UserService> _logger;

	public UserService(IMapper mapper, IUserRepository userRepository, ILogger<UserService> logger)
	{
		_mapper = mapper;
		_userRepository = userRepository;
		_logger = logger;
	}

	public async ValueTask<bool> AddUser(UserDto userDto)
	{
		var user = _mapper.Map<User>(userDto);

		if(!(await CheckIfUserExists(user)))
		{
			try
			{
				var result = await _userRepository.AddAsync(user);

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

		return false;
	}

	public async ValueTask<bool> RemoveUserById(long Id)
	{
		var entityId = (await _userRepository.GetByConditionAsync(x => x.TelegramId == Id)).FirstOrDefault()?.Id;

		if (entityId != null)
		{
			try
			{
				var deletedEntity = await _userRepository.DeleteByIdAsync(entityId.Value);
				
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error: {message}\nStack trace: {stackTrace}", ex.Message , ex.StackTrace);

				return false;
			}
		}

		return false;
	}

	private async ValueTask<bool> CheckIfUserExists(User user)
	{
		return (await _userRepository.GetByConditionAsync(x => x.TelegramId == user.TelegramId)).Any();
	}
}