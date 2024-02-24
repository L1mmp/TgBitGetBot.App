using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Models;


namespace TgBitGetBot.Infrastructure.Services;

public class TickerService : ITickerService
{
	private readonly IMapper _mapper;
	private readonly HttpClient _httpClient;
	private readonly ILogger<TickerService> _logger;

	public TickerService(IMapper mapper, ILogger<TickerService> logger, IHttpClientFactory factory)
	{
		_mapper = mapper;
		_logger = logger;
		_httpClient = factory.CreateClient(HttpClientConstNames.BitGetApiName);
	}

	public async Task<IEnumerable<TickerModel>> GetTickersInfo()
	{
		var topTickers = await _httpClient.GetFromJsonAsync<TickerRequsetDto>("spot/v1/market/tickers");

		if (topTickers is null)
		{
			_logger.LogError("{Result}. Current time: {dateTime}",
				"Tickers info",
				DateTime.UtcNow);

			return Enumerable.Empty<TickerModel>();
		}

		var usdtTopDepth = topTickers.Data!.Where(x => x.Symbol!.EndsWith("USDT"));

		return _mapper.Map<List<TickerModel>>(usdtTopDepth);
	}

	public async Task<string> GetTopTickers()
	{
		var topTickers = await _httpClient.GetFromJsonAsync<TickerRequsetDto>("spot/v1/market/tickers");

		if (topTickers is null)
		{
			_logger.LogError("{Result}. Current time: {dateTime}",
				"Top tickers not found",
				DateTime.UtcNow);

			return "Top tickers not found";
		}

		var usdtTopDepth = topTickers.Data!.Where(x => x.Symbol!.EndsWith("USDT"));

		var listTickers = _mapper.Map<List<TickerModel>>(usdtTopDepth);

		var top = listTickers
			.OrderByDescending(x => x.UsdtVol)
			.Take(5)
			.Select(y => $"● {y.Symbol} \n\t Depth:{Math.Round(y.UsdtVol, 1)}$ \n\t Price: {y.Close}$")
			.ToList();

		var result = string.Join("\n", top);

		return result;
	}


}