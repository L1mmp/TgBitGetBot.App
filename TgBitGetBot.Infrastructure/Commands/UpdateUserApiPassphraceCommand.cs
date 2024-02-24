using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Attributes;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Enums;

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


			var keyboard = new ReplyKeyboardMarkup("")
			{
				Keyboard = new[]
				{
					new[]
					{
						new KeyboardButton(CommandNames.RegisterUserApiCommandName),
						new KeyboardButton(CommandNames.GetTopTickersByDepthCommandName),
						new KeyboardButton(CommandNames.UnRegisterUserCommandName)
					}
				}
			};

			await dbTask.ContinueWith(task =>
			{
				var _message = botClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "API успешно добавлено!",
					replyMarkup: keyboard,
					cancellationToken: ct);
			}, ct);
		}
	}
}
