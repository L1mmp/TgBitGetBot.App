using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgBitGetBot.Infrastructure.TelegramMessageListener;

public interface ITelegramMessageListener
{
	public Task StartListening();
	public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

	public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
}