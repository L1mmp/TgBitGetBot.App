namespace TgBitGetBot.Domain.Dtos;

public class TickerSoloRequsetDto
{
    public string code { get; set; }
    public string msg { get; set; }
    public TickerDto data { get; set; }
}