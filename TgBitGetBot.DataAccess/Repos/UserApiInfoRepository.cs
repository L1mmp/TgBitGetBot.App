using TgBitGetBot.DataAccess.Repos.Interfaces;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.DataAccess.Repos;

public class UserApiInfoRepository: BaseRepository<UserApiInfo>, IUserApiInfoRepository
{
	public UserApiInfoRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
	}
}