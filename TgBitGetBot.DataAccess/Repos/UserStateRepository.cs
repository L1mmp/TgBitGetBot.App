using TgBitGetBot.DataAccess.Repos.Interfaces;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.DataAccess.Repos
{
	public class UserStateRepository : BaseRepository<UserState>, IUserStateRepository
	{
		public UserStateRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
		}
	}
}
