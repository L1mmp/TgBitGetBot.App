using RestSharp;
using Telegram.Bot;
using TgBitGetBot.App.Utils;
using System.Text.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Models;
using TgBitGetBot.Infrastructure.Services;

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

DateTimeOffset now = DateTimeOffset.Now;
long millisecondsSinceEpoch = now.ToUnixTimeMilliseconds();

var signature = new SignatureUtil().Generate(millisecondsSinceEpoch.ToString(), 
											"GET",
											"api/spot/v1/market/tickers",
											"", 
											null,
											"");

Console.WriteLine(signature);

var client = new RestClient("https://api.bitget.com/api/spot/v1/market/tickers");
var request = new RestRequest(client.Options.BaseUrl?.ToString(),Method.Get);
request.AddHeader("ACCESS-KEY", "bg_b4c177b11f2fdc12271f1c2de5a1fec8");
request.AddHeader("ACCESS-SIGN", signature);
request.AddHeader("ACCESS-PASSPHRASE", "ZzNY7zZw07lxs6cd");
request.AddHeader("ACCESS-TIMESTAMP", millisecondsSinceEpoch);
request.AddHeader("locale", "en-US");
request.AddHeader("Content-Type", "application/json");
request.AddHeader("Cookie", "__cf_bm=XI._TyRcJUg1Ew2ht2KMzHM.xCG4tJB_jlk13PFn84g-1677329710-0-AUQaQ7D6cfYUQdvu92i16pxDo6ntH/17eviXCFO2VvwQGlddgtVqwP9xCyzrCbrAhFqHawBGUJD599lzFRK/Cqw=");
var response = client.Execute(request);

string json = @"[
        {
            ""symbol"": ""LINKUSDT"",
            ""high24h"": ""7.677"",
            ""low24h"": ""7.1774"",
            ""close"": ""7.2696"",
            ""quoteVol"": ""3519964.6697"",
            ""baseVol"": ""476012.8389"",
            ""usdtVol"": ""3519964.66960815"",
            ""ts"": ""1677358281493"",
            ""buyOne"": ""7.2667"",
            ""sellOne"": ""7.2723"",
            ""bidSz"": ""85.807"",
            ""askSz"": ""264.033"",
            ""openUtc0"": ""7.4672"",
            ""changeUtc"": ""-0.02646"",
            ""change"": ""-0.01074""
        },
        {
            ""symbol"": ""UNIUSDT"",
            ""high24h"": ""6.6251"",
            ""low24h"": ""6.2796"",
            ""close"": ""6.3318"",
            ""quoteVol"": ""2357567.2682"",
            ""baseVol"": ""360238.5949"",
            ""usdtVol"": ""2357567.26811131"",
            ""ts"": ""1677358281488"",
            ""buyOne"": ""6.3311"",
            ""sellOne"": ""6.3427"",
            ""bidSz"": ""76.1954"",
            ""askSz"": ""35.64"",
            ""openUtc0"": ""6.5738"",
            ""changeUtc"": ""-0.03681"",
            ""change"": ""-0.02676""
        }]";

var listTickers = JsonSerializer.Deserialize<List<TickerDto>>(json);

Console.WriteLine(response.Content);


async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
	// Only process Message updates: https://core.telegram.org/bots/api#message
	if (update.Message is not { } message)
		return;
	// Only process text messages
	if (message.Text is not { } messageText)
		return;


	if (message.Text == "/top5")
	{
		
	}

	var chatId = message.Chat.Id;

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