using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Domain.Attributes;
using TgBitGetBot.Domain.Consts;

namespace TgBitGetBot.Infrastructure.Commands
{
	[CommandKey(CommandNames.DefaultCommandName)]
	public class DefaultCommand : IKeyCommand
	{
		public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = new())
		{
			await botClient.SendTextMessageAsync(
				message.Chat.Id,
				"Выберите действие.",
				cancellationToken: ct);
		}
	}
}
