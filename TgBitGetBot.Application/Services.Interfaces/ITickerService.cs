using TgBitGetBot.Domain.Models;

namespace TgBitGetBot.Application.Services.Interfaces;

public interface ITickerService
{
	public Task<string> GetTopTickers();
	public Task<IEnumerable<TickerModel>> GetTickersInfo();
}