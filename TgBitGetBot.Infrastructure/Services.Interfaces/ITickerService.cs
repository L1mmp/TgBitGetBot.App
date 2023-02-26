namespace TgBitGetBot.Infrastructure.Services.Interfaces;

public interface ITickerService
{
	public Task<string> GetTopTickers();
}