using EasyCaching.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Confgis;
using TgBitGetBot.Infrastructure.Services;

namespace TgBitGetBot.Infrastructure.Workers
{
	public class TickerChangeTrackerWorker : BackgroundService
	{
		private static string Schedule => "*/1 * * * * *"; //Runs every 10 seconds
		private readonly ITickerService _tickerService;
		private readonly TickerChangeTrackerService _tickerChangeTrackerService;
		private readonly IEasyCachingProvider _cacheProvider;
		private readonly ILogger<TopTickersWorker> _logger;
		private readonly CrontabSchedule _schedule;
		private DateTime _nextRun;

		/// <summary>
		/// Срок жизни кэшированных значений в минутах
		/// </summary>
		private readonly int _cacheExpirationPeriodMin;

		public TickerChangeTrackerWorker(
			ITickerService tickerService,
			IEasyCachingProvider provider,
			ILogger<TopTickersWorker> logger,
			IOptions<EasyCacheConfigs> options)
		{
			_tickerService = tickerService;
			_cacheProvider = provider;
			_schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
			_nextRun = _schedule.GetNextOccurrence(DateTime.Now);
			_logger = logger;
			_cacheExpirationPeriodMin = options.Value.inmemory!.DefaultLifetimeMin;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			do
			{
				var now = DateTime.Now;

				_nextRun = _schedule.GetNextOccurrence(now);

				if (now > _nextRun)
				{
					await Process();
					_nextRun = _schedule.GetNextOccurrence(DateTime.Now);
				}
				await Task.Delay(5000, stoppingToken); //5 seconds delay
			}
			while (!stoppingToken.IsCancellationRequested);
		}

		private async Task Process()
		{

			var tickersInfo = await _tickerService.GetTickersInfo();

			var tickersChanged = await _tickerChangeTrackerService.GetTickersChanged(tickersInfo);
			// Get tickers info changed
			// Call UserNotify service (Tickers)
			// Notifies users.

			try
			{

			}
			catch (Exception ex)
			{
				_logger.LogError("Error setting into cahce.\nError: {execption}. Stack trace:{stackTrace} Current time: {dateTime}",
					ex.Message,
					ex.StackTrace,
					DateTime.UtcNow
					);
			}

		}
	}
}
