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
	[CommandState(TelegramDialogState.WaitingForPassphrace)]
	public class UpdateUserApiPassphraceCommand : IStateCommand
	{
		private readonly IUserApiInfoService _userApiInfoService;
		private readonly IUserService _userService;
		private readonly IUserStateService _userStateService;

		public UpdateUserApiPassphraceCommand(IUserApiInfoService userApiInfoService, IUserStateService userStateService, IUserService userService)
		{
			_userApiInfoService = userApiInfoService;
			_userStateService = userStateService;
			_userService = userService;
		}

		public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = new())
		{
			var entity = await _userApiInfoService.GetLatestUserApiInfo(message.Chat.Id);

			entity.Passphrase = message.Text;

			var dbTask = _userApiInfoService.UpdateUserApiInfo(entity).ContinueWith(task =>
			{
				_userStateService.UpdateUserState(message.Chat.Id, TelegramDialogState.DefaultSatate);
			}, ct);

			await dbTask.ContinueWith(task =>
			{
				var _message = botClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "API успешно добавлено!",
					cancellationToken: ct);
			}, ct);
		}
	}
}
