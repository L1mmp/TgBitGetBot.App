using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using AutoMapper;
using RestSharp;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Models;
using TgBitGetBot.Infrastructure.Services.Interfaces;

namespace TgBitGetBot.Infrastructure.Services;

public class TickerService: ITickerService
{
	private readonly IMapper _mapper;

	public TickerService(IMapper mapper)
	{
		_mapper = mapper;
	}

	public async Task<string> GetTopTickers()
	{
		var options = new RestClientOptions(ApiRouteConsts.ApiRoute)
		{
			MaxTimeout = -1,
		};
		var client = new RestClient(options);
		var request = new RestRequest("/spot/v1/market/tickers", Method.Get);
		request.AddHeader("Cookie", "__cf_bm=I.VLicNg6QsWKfE5sv94ziTTvDTkQrijC0BW2KQblGE-1677404965-0-AWytVarts3nuB8CmFx9B2n1rgWgMlbov2tSxyKBSJaC0qILJlDSH12v+ervcplvF6dHopsC9fg3t84Yvrs04qCE=");
		request.AddHeader("Accept-Encoding", "UTF-8");

		var response = await client.ExecuteAsync(request);

		var serilized = JsonConvert.DeserializeObject<TickerRequsetDto>(response.Content)?
			.data.Where(x => x.symbol.EndsWith("USDT"));

		var listTickers = _mapper.Map<List<TickerModel>>(serilized);

		var top = listTickers
			.OrderByDescending(x => x.UsdtVol)
			.Take(5)
			.Select(y => $"{y.Symbol} - {Math.Round(y.UsdtVol, 1)}$")
			.ToList();

		return string.Join("\n", top);
	}
}