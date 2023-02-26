using TgBitGetBot.DataAccess.Repos.Interfaces;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.DataAccess.Repos;

public class UserRepository : BaseRepository<User>, IUserRepository
{
	public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
	}
}