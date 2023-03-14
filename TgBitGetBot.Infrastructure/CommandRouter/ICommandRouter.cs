using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgBitGetBot.Infrastructure.CommandRouter;

public interface ICommandRouter
{
	public Task ExecuteCommand(Message message, ITelegramBotClient botClient);
}