using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Attributes;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Enums;
using TgBitGetBot.Infrastructure.Services;

namespace TgBitGetBot.Infrastructure.Commands
{
	[CommandState(TelegramDialogState.WaitingForToken)]
	public class UpdateUserApiTokenCommand : IStateCommand
	{
		private readonly IUserApiInfoService _userApiInfoService;
		private readonly IUserService _userService;
		private readonly IUserStateService _userStateService;

		public UpdateUserApiTokenCommand(IUserApiInfoService userApiInfoService, IUserStateService userStateService, IUserService userService)
		{
			_userApiInfoService = userApiInfoService;
			_userStateService = userStateService;
			_userService = userService;
		}

		public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = new())
		{
			var entity = await _userApiInfoService.GetLatestUserApiInfo(message.Chat.Id);

			entity.Token = message.Text;

			var dbTask = _userApiInfoService.UpdateUserApiInfo(entity).ContinueWith(task =>
			{
				_userStateService.UpdateUserState(message.Chat.Id, TelegramDialogState.WaitingForPassphrace);
			}, ct);

			await dbTask.ContinueWith(task =>
			{
				var _message = botClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "Token успешно установлен.\nПожалуйста введите Passphrace:",
					cancellationToken: ct);
			}, ct);


		}
	}
}
