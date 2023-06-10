using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgBitGetBot.Application.CommandRouter.Interface;

public interface ICommandRouter
{
	public Task ExecuteCommand(Message message, ITelegramBotClient botClient);
}