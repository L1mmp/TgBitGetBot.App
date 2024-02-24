using Microsoft.Extensions.DependencyInjection;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Infrastructure.Commands;

namespace TgBitGetBot.App.Extensions
{
	public static partial class ServiceCollectionExtensions
	{
		public static IServiceCollection AddKeyCommands(this IServiceCollection services)
		{
			services.AddTransient<IKeyCommand, RegisterUserCommand>();
			services.AddTransient<IKeyCommand, UnRegisterUserCommand>();
			services.AddTransient<IKeyCommand, RegisterUserApiCommand>();
			services.AddTransient<IKeyCommand, GetTopTickersByDepthCommand>();
			services.AddTransient<IKeyCommand, DefaultCommand>();
			services.AddTransient<IKeyCommand, ResetStateCommand>();
			services.AddTransient<IKeyCommand, NeedToRegisterUserCommand>();

			return services;
		}

		public static IServiceCollection AddStateCommands(this IServiceCollection services)
		{
			services.AddTransient<IStateCommand, UpdateUserApiTokenCommand>();
			services.AddTransient<IStateCommand, UpdateUserApiPassphraceCommand>();

			return services;
		}
	}
}
