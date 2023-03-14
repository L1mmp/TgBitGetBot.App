using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Infrastructure.Services.Interfaces;

namespace TgBitGetBot.Infrastructure.Commands
{
	internal class GetTopTickersByDepthCommand : ICommand
	{
		private readonly ITickerService _tickerService;
		private readonly Message _message;
		private readonly ITelegramBotClient _botClient;

		public GetTopTickersByDepthCommand(ITickerService tickerService, Message message, ITelegramBotClient botClient)
		{
			_tickerService = tickerService;
			_message = message;
			_botClient = botClient;
		}
		public async Task Execute()
		{
			using CancellationTokenSource cts = new();
			var topString = await _tickerService.GetTopTickers();

			var message = await _botClient.SendTextMessageAsync(
				chatId: _message.Chat.Id,
				text: $"{topString}",
				cancellationToken: cts.Token);
		}

		public Task UnExecute()
		{
			throw new NotImplementedException();
		}
	}
}
