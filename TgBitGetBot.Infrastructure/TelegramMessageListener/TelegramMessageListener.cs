using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Infrastructure.CommandRouter;
using TgBitGetBot.Infrastructure.Services;
using TgBitGetBot.Infrastructure.Services.Interfaces;

namespace TgBitGetBot.Infrastructure.TelegramMessageListener;

public class TelegramMessageListener : ITelegramMessageListener
{
	private readonly ILogger<TelegramMessageListener> _logger;
	private readonly IConfiguration _config;
	private readonly ITickerService _tickerService;
	private readonly IUserService _userService;
	private readonly ICommandRouter _router;


	public TelegramMessageListener(
		ILogger<TelegramMessageListener> logger, 
		IConfiguration config, 
		ITickerService tickerService, 
		IUserService userService,
		ICommandRouter router)
	{
		_logger = logger;
		_config = config;
		_tickerService = tickerService;
		_userService = userService;
		_router = router;
	}

	public async Task StartListening()
	{
		var botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("token"));

		using CancellationTokenSource cts = new();

		// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
		ReceiverOptions receiverOptions = new()
		{
			AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
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

		// Send cancellation request to stop bot
		cts.Cancel();
	}

	public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
	{
		await _router.ExecuteCommand(update.Message, botClient);

		_logger.LogInformation($"Received a '{update.Message.Text}' message in chat {update.Message.Chat.Id}.");

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