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
			.ForMember(d => d.Symbol, opt => opt.MapFrom(src => src.symbol))
			.ForMember(d => d.High24h, opt => opt.MapFrom(src => decimal.Parse(src.high24h, CultureInfo.InvariantCulture)))
			.ForMember(d => d.AskSz, opt => opt.MapFrom(src => decimal.Parse(src.askSz, CultureInfo.InvariantCulture)))
			.ForMember(d => d.BaseVol, opt => opt.MapFrom(src => decimal.Parse(src.baseVol, CultureInfo.InvariantCulture)))
			.ForMember(d => d.BidSz, opt => opt.MapFrom(src => decimal.Parse(src.bidSz, CultureInfo.InvariantCulture)))
			.ForMember(d => d.BuyOne, opt => opt.MapFrom(src => decimal.Parse(src.buyOne, CultureInfo.InvariantCulture)))
			.ForMember(d => d.Change, opt => opt.MapFrom(src => decimal.Parse(src.change, CultureInfo.InvariantCulture)))
			.ForMember(d => d.ChangeUtc, opt => opt.MapFrom(src => decimal.Parse(src.changeUtc, CultureInfo.InvariantCulture)))
			.ForMember(d => d.Close, opt => opt.MapFrom(src => decimal.Parse(src.close, CultureInfo.InvariantCulture)))
			.ForMember(d => d.Low24h, opt => opt.MapFrom(src => decimal.Parse(src.low24h, CultureInfo.InvariantCulture)))
			.ForMember(d => d.OpenUtc0, opt => opt.MapFrom(src => decimal.Parse(src.openUtc0, CultureInfo.InvariantCulture)))
			.ForMember(d => d.QuoteVol, opt => opt.MapFrom(src => decimal.Parse(src.quoteVol, CultureInfo.InvariantCulture)))
			.ForMember(d => d.SellOne, opt => opt.MapFrom(src => decimal.Parse(src.sellOne, CultureInfo.InvariantCulture)))
			.ForMember(d => d.UsdtVol, opt => opt.MapFrom(src => decimal.Parse(src.usdtVol, CultureInfo.InvariantCulture)))
			.ForMember(d => d.Ts, opt => opt.MapFrom(src => DateTime.UnixEpoch.Add(TimeSpan.FromMilliseconds(double.Parse(src.ts)))));
		//CreateMap<TickerDto, TickerModel>()
		//	.ForMember(d => d.Symbol, opt => opt.MapFrom(src => src.symbol))
		//	.ForMember(d => d.High24h, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.AskSz, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.BaseVol, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.BidSz, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.BuyOne, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.Change, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.ChangeUtc, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.Close, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.Low24h, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.OpenUtc0, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.QuoteVol, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.SellOne, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.UsdtVol, opt => opt.MapFrom<MyResolver>())
		//	.ForMember(d => d.Ts, opt => opt.MapFrom(src => DateTime.UnixEpoch.Add(TimeSpan.FromMilliseconds(double.Parse(src.ts)))));
	}


	public class MyResolver : AutoMapper.IValueResolver<TickerDto, TickerModel, decimal>
	{
		public decimal Resolve(TickerDto source, TickerModel destination, decimal destMember, AutoMapper.ResolutionContext context)
		{
			switch (nameof(destMember))
			{
				case nameof(destination.Symbol): return destMember;
			}
			//return decimal.TryParse(source.high24h, out var result) ? result : default(decimal);
			return decimal.Parse(source.high24h, CultureInfo.InvariantCulture);
		}
	}
}