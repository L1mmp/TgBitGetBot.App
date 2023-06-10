using System.Text.Json.Serialization;

namespace TgBitGetBot.Domain.Dtos;

public class TickerRequsetDto
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

	[JsonPropertyName("msg")]
	public string? Msg { get; set; }

	[JsonPropertyName("requestTime")]
	public long RequestTime { get; set; }

	[JsonPropertyName("data")]
	public List<TickerDto> Data { get; set; }
}