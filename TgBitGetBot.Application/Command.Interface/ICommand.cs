using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgBitGetBot.Application.Command.Interface;

public interface ICommand
{
	public Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = new());

}