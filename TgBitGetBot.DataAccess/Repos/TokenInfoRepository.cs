using TgBitGetBot.Domain.Entities;
using TgBitGetBot.Infrastructure.Services;

namespace TgBitGetBot.DataAccess.Repos
{
	internal class TokenInfoRepository : BaseRepository<TokenInfo>, ITokenInfoRepository
	{
		public TokenInfoRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
		}
	}
}
