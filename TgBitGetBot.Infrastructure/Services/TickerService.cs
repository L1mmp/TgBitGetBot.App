using System.Text.Json;
using RestSharp;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Infrastructure.Services.Interfaces;

namespace TgBitGetBot.Infrastructure.Services;

public class TickerService: ITickerService
{
	public async Task<string> GetTopTickers()
	{
		var options = new RestClientOptions(ApiRouteConsts.ApiRoute)
		{
			MaxTimeout = -1,
		};
		var client = new RestClient(options);
		var request = new RestRequest("/spot/v1/market/tickers", Method.Get);
		request.AddHeader("Cookie", "__cf_bm=I.VLicNg6QsWKfE5sv94ziTTvDTkQrijC0BW2KQblGE-1677404965-0-AWytVarts3nuB8CmFx9B2n1rgWgMlbov2tSxyKBSJaC0qILJlDSH12v+ervcplvF6dHopsC9fg3t84Yvrs04qCE=");
		RestResponse response = await client.ExecuteAsync(request);
		
		var listTickers = JsonSerializer.Deserialize<TickerRequsetDto>(response.Content);

		var top = listTickers.data
			.OrderByDescending(x => Math.Round(Convert.ToDecimal(x.usdtVol),2))
			.Take(5)
			.Select(y => $"{y.symbol} - {Math.Round(Convert.ToDecimal(y.usdtVol),2)}$");

		return string.Join("\n", top);
	}
}