namespace TgBitGetBot.Domain.Models;

public class TickerModel
{
	public string? Symbol { get; set; }
	public decimal High24h { get; set; }
	public decimal Low24h { get; set; }
	public decimal Close { get; set; }
	public decimal QuoteVol { get; set; }
	public decimal BaseVol { get; set; }
	public decimal UsdtVol { get; set; }
	public DateTime Ts { get; set; }
	public decimal BuyOne { get; set; }
	public decimal SellOne { get; set; }
	public decimal BidSz { get; set; }
	public decimal AskSz { get; set; }
	public decimal OpenUtc0 { get; set; }
	public decimal ChangeUtc { get; set; }
	public decimal Change { get; set; }
}