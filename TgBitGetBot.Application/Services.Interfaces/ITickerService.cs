namespace TgBitGetBot.Application.Services.Interfaces;

public interface ITickerService
{
	public Task<string> GetTopTickers();
}