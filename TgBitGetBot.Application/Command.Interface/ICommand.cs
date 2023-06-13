using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Factories.Interface;

namespace TgBitGetBot.Application.Command.Interface;

public interface ICommand
{
	public Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken ct = new());

}