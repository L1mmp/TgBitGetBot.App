using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBitGetBot.Application.CommandRouter.Interface;
using TgBitGetBot.Application.TelegramMessageListener.Interface;

namespace TgBitGetBot.Infrastructure.TelegramMessageListener;

public class TelegramMessageListener : ITelegramMessageListener
{
	private readonly ILogger _logger;
	private readonly ICommandRouter _router;
	private readonly ITelegramBotClient _botClient;

	public TelegramMessageListener(
		ILoggerFactory loggerFactory,
		ICommandRouter router,
		ITelegramBotClient botClient)
	{
		_logger = loggerFactory.CreateLogger<TelegramMessageListener>();
		_router = router;
		_botClient = botClient;
	}

	public async Task StartListening()
	{
		using CancellationTokenSource cts = new();

		ReceiverOptions receiverOptions = new()
		{
			AllowedUpdates = Array.Empty<UpdateType>()
		};

		_botClient.StartReceiving(
		   updateHandler: HandleUpdateAsync,
		   pollingErrorHandler: HandlePollingErrorAsync,
		   receiverOptions: receiverOptions,
		   cancellationToken: cts.Token
	   );

		var me = await _botClient.GetMeAsync(cts.Token);

		Console.WriteLine($"Start listening for @{me.Username}");
		Console.ReadLine();
	}

	public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
	{
		try
		{
			await _router.ExecuteCommand(update.Message, botClient);

			_logger.LogInformation("Received a '{text}' message in chat {chatId}.",
				update.Message.Text,
				update.Message.Chat.Id);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message);
			throw;
		}
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