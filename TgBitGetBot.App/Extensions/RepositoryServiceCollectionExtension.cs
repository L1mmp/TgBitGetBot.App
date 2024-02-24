using Microsoft.Extensions.DependencyInjection;
using TgBitGetBot.DataAccess.Repos;
using TgBitGetBot.DataAccess.Repos.Interfaces;

namespace TgBitGetBot.App.Extensions
{
	public static partial class ServiceCollectionExtensions
	{
		public static IServiceCollection AddRepositories(this IServiceCollection services)
		{
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IUserStateRepository, UserStateRepository>();
			services.AddTransient<IUserApiInfoRepository, UserApiInfoRepository>();

			return services;
		}
	}
}
