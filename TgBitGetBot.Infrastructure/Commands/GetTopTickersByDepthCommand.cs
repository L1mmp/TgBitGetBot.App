using EasyCaching.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Domain.Attributes;
using TgBitGetBot.Domain.Consts;

namespace TgBitGetBot.Infrastructure.Commands
{
	[CommandKey(CommandNames.GetTopTickersByDepthCommandName)]
	public class GetTopTickersByDepthCommand : IKeyCommand
	{
		private readonly IEasyCachingProvider _provider;

		public GetTopTickersByDepthCommand(IEasyCachingProvider provider)
		{
			_provider = provider;
		}
		public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = new())
		{
			var topString = await _provider.GetAsync<string>("topTickers");

			var _message = await botClient.SendTextMessageAsync(
				chatId: message.Chat.Id,
				text: $"{topString}",
				cancellationToken: ct);
		}
	}
}
