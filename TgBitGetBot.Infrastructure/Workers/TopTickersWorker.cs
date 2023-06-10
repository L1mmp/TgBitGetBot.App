using EasyCaching.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Confgis;

namespace TgBitGetBot.Infrastructure.Workers
{
	public class TopTickersWorker : BackgroundService
	{
		private static string Schedule => "*/10 * * * * *"; //Runs every 10 seconds
		private readonly ITickerService _tickerService;
		private readonly IEasyCachingProvider _cacheProvider;
		private readonly ILogger<TopTickersWorker> _logger;
		private readonly CrontabSchedule _schedule;
		private DateTime _nextRun;

		/// <summary>
		/// Срок жизни кэшированных значений в минутах
		/// </summary>
		private readonly int _cacheExpirationPeriodMin;

		public TopTickersWorker(
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
			_cacheExpirationPeriodMin = options.Value.inmemory.DefaultLifetimeMin;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			do
			{
				var now = DateTime.Now;
				var nextrun = _schedule.GetNextOccurrence(now);
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
			
			var topTickers = await _tickerService.GetTopTickers();

			try
			{
				await _cacheProvider.SetAsync<string>(
					"topTickers", 
					topTickers, 
					TimeSpan.FromMinutes(_cacheExpirationPeriodMin)
					);
				_logger.LogInformation(topTickers);
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
