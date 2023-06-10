using Telegram.Bot.Types;
using Telegram.Bot;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Enums;
using TgBitGetBot.Application.Factories.Interface;
using Microsoft.Extensions.DependencyInjection;
using TgBitGetBot.Infrastructure.Services;

namespace TgBitGetBot.Infrastructure.Commands
{
	internal class RegisterUserApiCommand : ICommand
	{
		private readonly IUserApiInfoService _userApiInfoService;
		private readonly IUserStateService _userStateService;

		public RegisterUserApiCommand(IUserApiInfoService userApiInfoService, IUserStateService userStateService)
		{
			_userApiInfoService = userApiInfoService;
			_userStateService = userStateService;
		}

		public async Task Execute(Message message, ITelegramBotClient botClient)
		{
			var _message = await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
				text: "Пожалуйста введите Token:");

			await _userStateService.UpdateUserState(message.Chat.Id, TelegramDialogState.WaitingForToken);
		}

		public async Task UnExecute()
		{
			await Task.Run(() =>
				{
					throw new NotImplementedException();
				});
		}
	}
}
