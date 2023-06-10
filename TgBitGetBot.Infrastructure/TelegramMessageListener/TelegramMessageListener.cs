using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBitGetBot.Application.CommandRouter.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Application.TelegramMessageListener.Interface;

namespace TgBitGetBot.Infrastructure.TelegramMessageListener;

public class TelegramMessageListener : ITelegramMessageListener
{
	private readonly ILogger _logger;
	private readonly ICommandRouter _router;

	public TelegramMessageListener(
		ILoggerFactory loggerFactory,
		ICommandRouter router)
	{
		_logger = loggerFactory.CreateLogger<TelegramMessageListener>();
		_router = router;
	}

	public async Task StartListening()
	{
		var token = Environment.GetEnvironmentVariable("token");

		if (token is null)
		{
			_logger.LogError("Token is not set in enviroment or incorrect");
		}

		var botClient = new TelegramBotClient(token!);

		using CancellationTokenSource cts = new();

		ReceiverOptions receiverOptions = new()
		{
			AllowedUpdates = Array.Empty<UpdateType>()
		};

		botClient.StartReceiving(
		   updateHandler: HandleUpdateAsync,
		   pollingErrorHandler: HandlePollingErrorAsync,
		   receiverOptions: receiverOptions,
		   cancellationToken: cts.Token
	   );

		var me = await botClient.GetMeAsync(cts.Token);

		Console.WriteLine($"Start listening for @{me.Username}");
		Console.ReadLine();

		cts.Cancel();
	}

	public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
	{
		await _router.ExecuteCommand(update.Message!, botClient);

		_logger.LogInformation("Received a '{text}' message in chat {chatId}.", 
			update.Message!.Text, 
			update.Message.Chat.Id);

	}

	public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
	{
		var ErrorMessage = exception switch
		{
			ApiRequestException apiRequestException
				=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
			_ => exception.ToString()
		};

		_logger.LogError(ErrorMessage);
		return Task.CompletedTask;
	}
}