namespace TgBitGetBot.Domain.Dtos;

public class TickerRequsetDto
{
    public string code { get; set; }
    public string msg { get; set; }
    public long requestTime { get; set; }
    public List<TickerDto> data { get; set; }
}