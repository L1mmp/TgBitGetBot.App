using System.Text.Json.Serialization;

namespace TgBitGetBot.Domain.Dtos;

public class TickerDto
{
	[JsonPropertyName("symbol")]
	public string? Symbol { get; set; }

	[JsonPropertyName("high24h")]
	public string? High24h { get; set; }

	[JsonPropertyName("low24h")]
	public string? Low24h { get; set; }

	[JsonPropertyName("close")]
	public string? Close { get; set; }

	[JsonPropertyName("quoteVol")]
	public string? QuoteVol { get; set; }

	[JsonPropertyName("baseVol")]
	public string? BaseVol { get; set; }

	[JsonPropertyName("usdtVol")]
	public string? UsdtVol { get; set; }

	[JsonPropertyName("ts")]
	public string? Ts { get; set; }

	[JsonPropertyName("buyOne")]
	public string? BuyOne { get; set; }

	[JsonPropertyName("sellOne")]
	public string? SellOne { get; set; }

	[JsonPropertyName("bidSz")]
	public string? BidSz { get; set; }

	[JsonPropertyName("askSz")]
	public string? AskSz { get; set; }

	[JsonPropertyName("openUtc0")]
	public string? OpenUtc0 { get; set; }

	[JsonPropertyName("changeUtc")]
	public string? ChangeUtc { get; set; }

	[JsonPropertyName("change")]
	public string? Change { get; set; }
}