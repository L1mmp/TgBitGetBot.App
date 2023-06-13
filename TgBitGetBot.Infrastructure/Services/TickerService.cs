using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using AutoMapper;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Models;
using Microsoft.Extensions.Logging;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.DataAccess.Repos.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;


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

	public async Task<string> GetTopTickers()
	{
		var topTickers = await _httpClient.GetFromJsonAsync<TickerRequsetDto>("spot/v1/market/tickers");

		if(topTickers is null)
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
			.Select(y => $"{y.Symbol} - {Math.Round(y.UsdtVol, 1)}$")
			.ToList();

		var result = string.Join("\n", top);

		return result;
	}
}