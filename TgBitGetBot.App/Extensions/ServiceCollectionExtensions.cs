using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Infrastructure.Services;

namespace TgBitGetBot.App.Extensions
{
	public static partial class ServiceCollectionExtensions
	{
		public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection)
		{
			var token = Environment.GetEnvironmentVariable("token");

			if (token is null)
			{
				Log.Error("Token is not set in enviroment or incorrect");
			}

			var client = new TelegramBotClient(Environment.GetEnvironmentVariable("token"));

			return serviceCollection
				.AddTransient<ITelegramBotClient>(x => client);
		}

		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddTransient<ITickerService, TickerService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IUserApiInfoService, UserApiInfoService>();
			services.AddTransient<IUserStateService, UserStateService>();

			return services;
		}
	}
}
