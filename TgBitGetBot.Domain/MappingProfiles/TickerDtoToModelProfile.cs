using System.Globalization;
using AutoMapper;
using TgBitGetBot.Domain.Dtos;
using TgBitGetBot.Domain.Entities;
using TgBitGetBot.Domain.Models;

namespace TgBitGetBot.Domain.MappingProfiles;

public class TickerDtoToModelProfile : Profile
{
	public TickerDtoToModelProfile()
	{
		CreateMap<TickerDto, TickerModel>()
			.ForMember(d => d.Symbol, opt => opt.MapFrom(src => src.Symbol))
			.ForMember(d => d.High24h, opt => opt.MapFrom(src => decimal.Parse(src.High24h, CultureInfo.InvariantCulture)))
			.ForMember(d => d.AskSz, opt => opt.MapFrom(src => decimal.Parse(src.AskSz, CultureInfo.InvariantCulture)))
			.ForMember(d => d.BaseVol, opt => opt.MapFrom(src => decimal.Parse(src.BaseVol, CultureInfo.InvariantCulture)))
			.ForMember(d => d.BidSz, opt => opt.MapFrom(src => decimal.Parse(src.BidSz, CultureInfo.InvariantCulture)))
			.ForMember(d => d.BuyOne, opt => opt.MapFrom(src => decimal.Parse(src.BuyOne, CultureInfo.InvariantCulture)))
			.ForMember(d => d.Change, opt => opt.MapFrom(src => decimal.Parse(src.Change, CultureInfo.InvariantCulture)))
			.ForMember(d => d.ChangeUtc, opt => opt.MapFrom(src => decimal.Parse(src.ChangeUtc, CultureInfo.InvariantCulture)))
			.ForMember(d => d.Close, opt => opt.MapFrom(src => decimal.Parse(src.Close, CultureInfo.InvariantCulture)))
			.ForMember(d => d.Low24h, opt => opt.MapFrom(src => decimal.Parse(src.Low24h, CultureInfo.InvariantCulture)))
			.ForMember(d => d.OpenUtc0, opt => opt.MapFrom(src => decimal.Parse(src.OpenUtc0, CultureInfo.InvariantCulture)))
			.ForMember(d => d.QuoteVol, opt => opt.MapFrom(src => decimal.Parse(src.QuoteVol, CultureInfo.InvariantCulture)))
			.ForMember(d => d.SellOne, opt => opt.MapFrom(src => decimal.Parse(src.SellOne, CultureInfo.InvariantCulture)))
			.ForMember(d => d.UsdtVol, opt => opt.MapFrom(src => decimal.Parse(src.UsdtVol, CultureInfo.InvariantCulture)))
			.ForMember(d => d.Ts, opt => opt.MapFrom(src => DateTime.UnixEpoch.Add(TimeSpan.FromMilliseconds(double.Parse(src.Ts)))));
	}
}