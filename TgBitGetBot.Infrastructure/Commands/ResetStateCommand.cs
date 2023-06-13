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
using TgBitGetBot.Domain.Consts;

namespace TgBitGetBot.Infrastructure.Commands
{
	[CommandKey(CommandNames.ResetCommandName)]
	public class ResetStateCommand : IKeyCommand
	{
		private readonly IUserStateService _userStateService;

		public ResetStateCommand(IUserStateService userStateService)
		{
			_userStateService = userStateService;
		}

		public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = default)
		{
			await _userStateService.UpdateUserState(message.Chat.Id, Domain.Enums.TelegramDialogState.DefaultSatate);

			await botClient.SendTextMessageAsync(
				message.Chat.Id,
				"Выберите действие.",
				cancellationToken: ct);
		}
	}
}
