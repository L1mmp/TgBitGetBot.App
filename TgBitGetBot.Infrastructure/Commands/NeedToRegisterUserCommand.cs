using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Domain.Attributes;
using TgBitGetBot.Domain.Consts;

namespace TgBitGetBot.Infrastructure.Commands
{
	[CommandKey(CommandNames.NeedToregisterUser)]
	public class NeedToRegisterUserCommand : IKeyCommand
	{
		public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = default)
		{
			await botClient.SendTextMessageAsync(
				chatId: message.Chat.Id,
				text: "Сначала нужно зарегистрировать пользователя.",
				cancellationToken: ct);
		}
	}
}
