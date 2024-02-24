using TgBitGetBot.Domain.Models;

namespace TgBitGetBot.Infrastructure.Services
{
	public interface ITickerChangeTrackerService
	{
		Task<IEnumerable<TickerChange>> GetTickersChanged(IEnumerable<TickerModel> tickersInfo);
	}
}