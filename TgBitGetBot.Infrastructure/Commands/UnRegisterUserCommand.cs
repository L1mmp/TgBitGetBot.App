using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Factories.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Dtos;

namespace TgBitGetBot.Infrastructure.Commands;

public class UnRegisterUserCommand : ICommand
{
	private readonly IUserService _userService;
	public UnRegisterUserCommand(IUserService userService)
	{
		_userService = userService;
	}
	public async Task Execute(Message message, ITelegramBotClient botClient)
	{
		using CancellationTokenSource cts = new();
		var sendMessage = await _userService.RemoveUserById(message.Chat.Id)
			? "Пользователь успешно удален."
			: "Пользователь не зарегистрирован в системе.";

		var _message = await botClient.SendTextMessageAsync(
			chatId: message.Chat.Id,
			text: sendMessage,
			cancellationToken: cts.Token);
	}

	public Task UnExecute()
	{
		throw new NotImplementedException();
	}
}