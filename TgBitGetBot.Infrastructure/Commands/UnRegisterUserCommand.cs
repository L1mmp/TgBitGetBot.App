using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Infrastructure.Services.Interfaces;

namespace TgBitGetBot.Infrastructure.Commands;

public class UnRegisterUserCommand : ICommand
{
	private readonly IUserService _userService;
	private readonly Message _message;
	private readonly ITelegramBotClient _botClient;

	public UnRegisterUserCommand(IUserService userService, Message message, ITelegramBotClient botClient)
	{
		_userService = userService;
		_message = message;
		_botClient = botClient;
	}

	public async Task Execute()
	{
		using CancellationTokenSource cts = new();
		var sendMessage = await _userService.RemoveUserById(_message.Chat.Id)
			? "Пользователь успешно удален."
			: "Пользователь не зарегистрирован в системе.";

		var message = await _botClient.SendTextMessageAsync(
			chatId: _message.Chat.Id,
			text: sendMessage,
			cancellationToken: cts.Token);
	}

	public Task UnExecute()
	{
		throw new NotImplementedException();
	}
}