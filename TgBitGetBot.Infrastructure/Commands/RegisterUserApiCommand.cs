using Telegram.Bot.Types;
using Telegram.Bot;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Enums;
using TgBitGetBot.Application.Factories.Interface;
using Microsoft.Extensions.DependencyInjection;
using TgBitGetBot.Infrastructure.Services;
using TgBitGetBot.Domain.Attributes;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Dtos;

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

			var dbTask = _userApiInfoService.AddUserApiInfo(dto).ContinueWith(task =>
			{
				_userStateService.UpdateUserState(message.Chat.Id, TelegramDialogState.WaitingForToken);
			}, ct);

			await dbTask.ContinueWith(task =>
			{
				var _message = botClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "Пожалуйста введите Token:",
					cancellationToken: ct);
			}, ct);

		}
	}
}
