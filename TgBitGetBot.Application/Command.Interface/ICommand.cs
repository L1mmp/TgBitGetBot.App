using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Factories.Interface;

namespace TgBitGetBot.Application.Command.Interface;

public interface ICommand
{
	public abstract Task Execute(Message message, ITelegramBotClient botClient);
	public abstract Task UnExecute();

}