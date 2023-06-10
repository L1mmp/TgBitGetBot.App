using EasyCaching.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Factories.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Infrastructure.Services;

namespace TgBitGetBot.Infrastructure.Commands
{
	public class GetTopTickersByDepthCommand : ICommand
	{
		private readonly IEasyCachingProvider _provider;

		public GetTopTickersByDepthCommand(IEasyCachingProvider provider)
		{
			_provider = provider;
		}
		public async Task Execute(Message message, ITelegramBotClient botClient)
		{
			using CancellationTokenSource cts = new();
			var topString = await _provider.GetAsync<string>("topTickers");

			var _message = await botClient.SendTextMessageAsync(
				chatId: message.Chat.Id,
				text: $"{topString}",
				cancellationToken: cts.Token);
		}

		public Task UnExecute()
		{
			throw new NotImplementedException();
		}
	}
}
