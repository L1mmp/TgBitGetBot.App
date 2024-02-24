using AutoMapper;
using Microsoft.Extensions.Logging;
using TgBitGetBot.Domain.Models;

namespace TgBitGetBot.Infrastructure.Services
{
	public class TickerChangeTrackerService : ITickerChangeTrackerService
	{
		private readonly IMapper _mapper;
		private readonly ITokenInfoRepository _tokenRepository;
		private readonly ILogger<TickerChangeTrackerService> _logger;

		public TickerChangeTrackerService(
			IMapper mapper,
			ITokenInfoRepository tokenRepository,
			ILogger<TickerChangeTrackerService> logger)
		{
			_mapper = mapper;
			_tokenRepository = tokenRepository;
			_logger = logger;
		}

		public async Task<IEnumerable<TickerChange>> GetTickersChanged(IEnumerable<TickerModel> tickersInfo)
		{
			throw new NotImplementedException();
		}
	}
}