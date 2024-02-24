using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Attributes;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Enums;

namespace TgBitGetBot.Infrastructure.Commands
{
	[CommandKey(CommandNames.RegisterUserApiCommandName)]
	public class RegisterUserApiCommand : IKeyCommand
	{
		private readonly IUserApiInfoService _userApiInfoService;
		private readonly IUserStateService _userStateService;
		private readonly IUserService _userService;

		public RegisterUserApiCommand(IUserApiInfoService userApiInfoService, IUserStateService userStateService, IUserService userService)
		{
			_userApiInfoService = userApiInfoService;
			_userStateService = userStateService;
			_userService = userService;
		}

		public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = new())
		{
			var user = await _userService.GetUserByTelegramId(message.Chat.Id);


			var dto = new UserApiInfoDto()
			{
				UserId = user.Id,
				CreatedOn = DateTime.UtcNow
			};

			await _userApiInfoService.AddUserApiInfo(dto);

			await _userStateService.UpdateUserState(message.Chat.Id, TelegramDialogState.WaitingForToken);

			await botClient.SendTextMessageAsync(
				chatId: message.Chat.Id,
				text: "Пожалуйста введите Token:",
				cancellationToken: ct);
		}
	}
}
