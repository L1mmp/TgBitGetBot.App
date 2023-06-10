using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
