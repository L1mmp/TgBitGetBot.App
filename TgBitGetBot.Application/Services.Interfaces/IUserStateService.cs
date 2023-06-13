using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgBitGetBot.Domain.Entities;
using TgBitGetBot.Domain.Enums;

namespace TgBitGetBot.Application.Services.Interfaces
{
	public interface IUserStateService
	{
		Task<UserState> GetCurrentStateOfUser(long id);
		Task UpdateUserState(long telegramId, TelegramDialogState state);
	}
}
