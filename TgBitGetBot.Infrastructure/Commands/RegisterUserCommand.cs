using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Factories.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Dtos;

namespace TgBitGetBot.Infrastructure.Commands;

public class RegisterUserCommand : ICommand
{
	private readonly IUserService _userService;
	public RegisterUserCommand(IUserService userService)
	{
		_userService = userService;
	}

	public  async Task Execute(Message message, ITelegramBotClient botClient)
	{
		using CancellationTokenSource cts = new();
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
			cancellationToken: cts.Token);
	}

	public Task UnExecute()
	{
		throw new NotImplementedException();
	}
}