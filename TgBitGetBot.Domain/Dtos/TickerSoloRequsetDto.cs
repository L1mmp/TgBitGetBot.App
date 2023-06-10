using System.Text.Json.Serialization;

namespace TgBitGetBot.Domain.Dtos;

public class TickerSoloRequsetDto
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

	[JsonPropertyName("msg")]
	public string? Msg { get; set; }

	[JsonPropertyName("data")]
	public TickerDto? Data { get; set; }
}