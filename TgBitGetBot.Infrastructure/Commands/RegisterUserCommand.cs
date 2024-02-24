using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Attributes;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Dtos;

namespace TgBitGetBot.Infrastructure.Commands;

[CommandKey(CommandNames.RegisterUserCommandName)]
public class RegisterUserCommand : IKeyCommand
{
	private readonly IUserService _userService;
	public RegisterUserCommand(IUserService userService)
	{
		_userService = userService;
	}

	public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = new())
	{
		var user = new UserDto()
		{
			TelegramId = message.Chat.Id,
			Name = message.Chat.Username
		};

		var sendMessage = await _userService.AddUser(user)
			? "Пользователь успешно зарегистрирован."
			: "Пользователь уже существует.";

		var _message = await botClient.SendTextMessageAsync(
			chatId: message.Chat.Id,
			text: sendMessage,
			cancellationToken: ct);
	}
}