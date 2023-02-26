using RestSharp;
using Telegram.Bot;
using System.Text.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Models;
using TgBitGetBot.Domain.Utils;
using TgBitGetBot.Infrastructure.Services;
using TgBitGetBot.Infrastructure.Services.Interfaces;

var botClient = new TelegramBotClient("5835418871:AAEQnMJ2cu173CkHC9HJrZcPQTdVKdQdIVc");

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

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();
//
// DateTimeOffset now = DateTimeOffset.Now;
// long millisecondsSinceEpoch = now.ToUnixTimeMilliseconds();
//
// var signature = new SignatureUtil().Generate(millisecondsSinceEpoch.ToString(), 
// 											"GET",
// 											"api/spot/v1/market/tickers",
// 											"", 
// 											null,
// 											"");
//
// Console.WriteLine(signature);
//
// var client = new RestClient($"{ApiRouteConsts.ApiRoute}/spot/v1/market/tickers");
// var request = new RestRequest(client.Options.BaseUrl?.ToString(),Method.Get);
// request.AddHeader("ACCESS-KEY", "bg_b4c177b11f2fdc12271f1c2de5a1fec8");
// request.AddHeader("ACCESS-SIGN", signature);
// request.AddHeader("ACCESS-PASSPHRASE", "ZzNY7zZw07lxs6cd");
// request.AddHeader("ACCESS-TIMESTAMP", millisecondsSinceEpoch);
// request.AddHeader("locale", "en-US");
// request.AddHeader("Content-Type", "application/json");
// request.AddHeader("Cookie", "__cf_bm=XI._TyRcJUg1Ew2ht2KMzHM.xCG4tJB_jlk13PFn84g-1677329710-0-AUQaQ7D6cfYUQdvu92i16pxDo6ntH/17eviXCFO2VvwQGlddgtVqwP9xCyzrCbrAhFqHawBGUJD599lzFRK/Cqw=");
// var response = client.Execute(request);
//
// ITickerService service = new TickerService();
//
// var topString = await service.GetTopTickers();
//
// Console.WriteLine(topString);



async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
	// Only process Message updates: https://core.telegram.org/bots/api#message
	if (update.Message is not { } message)
		return;
	// Only process text messages
	if (message.Text is not { } messageText)
		return;


	var chatId = message.Chat.Id;
	
	if (message.Text == "/top5")
	{
		
		ITickerService service = new TickerService();

		var topString = await service.GetTopTickers();

		Console.WriteLine(topString);
		
		Message top5TickerMessage = await botClient.SendTextMessageAsync(
			chatId: chatId,
			text: $"{topString}",
			cancellationToken: cancellationToken);
	}

	Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

	// Echo received message text
	Message sentMessage = await botClient.SendTextMessageAsync(
		chatId: chatId,
		text: $"You said:\n {messageText} in chat with id: {chatId}",
		cancellationToken: cancellationToken);
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
	var ErrorMessage = exception switch
	{
		ApiRequestException apiRequestException
			=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
		_ => exception.ToString()
	};

	Console.WriteLine(ErrorMessage);
	return Task.CompletedTask;
}