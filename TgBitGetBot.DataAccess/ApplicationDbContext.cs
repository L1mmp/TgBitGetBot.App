using Microsoft.EntityFrameworkCore;
using TgBitGetBot.Domain.Entities;

namespace TgBitGetBot.DataAccess;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{ }
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<UserApiInfo> UserApiInfos { get; set; } = null!;
	public DbSet<UserState> UserStates { get; set; } = null!;

	public DbSet<UserToNotify> userToNotifies { get; set; } = null!;
}