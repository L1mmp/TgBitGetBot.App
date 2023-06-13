using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TgBitGetBot.App.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection)
		{
			var token = Environment.GetEnvironmentVariable("token");

			if (token is null)
			{
				Log.Error("Token is not set in enviroment or incorrect");
			}

			var client = new TelegramBotClient(Environment.GetEnvironmentVariable("token")!);

			return serviceCollection
				.AddTransient<ITelegramBotClient>(x => client);
		}
	}
}
