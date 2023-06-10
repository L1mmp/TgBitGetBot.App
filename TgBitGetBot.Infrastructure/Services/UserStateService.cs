using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.DataAccess.Repos.Interfaces;
using TgBitGetBot.Domain.Enums;

namespace TgBitGetBot.Infrastructure.Services
{
	public class UserStateService : IUserStateService
	{
		private readonly IUserStateRepository _stateRepository;

		public UserStateService(IUserStateRepository stateRepository)
		{
			_stateRepository = stateRepository;
		}

		public async Task UpdateUserState(long telegramId, TelegramDialogState state)
		{
			var entities = await _stateRepository.GetByConditionAsync(x => x.TelegramId == telegramId);
			if (!entities.Any())
			{
				await _stateRepository.AddAsync(new () { State = state, TelegramId = telegramId });
			}
			else
			{
				await _stateRepository.UpdateAsync(entities.FirstOrDefault());
			}
		}
	}
}
