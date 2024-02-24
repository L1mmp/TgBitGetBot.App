using TgBitGetBot.Domain.Enums;

namespace TgBitGetBot.Infrastructure.Services
{
	public class TickerChange
	{
		public string Name { get; set; } = null!;
		public IEnumerable<NotifyType> NotifyTypes { get; set; } = Enumerable.Empty<NotifyType>();
		public decimal NewPrice { get; set; }
		public decimal LastPrice { get; set; }
		public decimal NewDepth { get; set; }
		public decimal LastDepth { get; set; }
		public double PriceChagePercent { get; set; }
		public double DepthChagePercent { get; set; }
	}
}