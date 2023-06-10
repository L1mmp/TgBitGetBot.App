using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.EntityFrameworkCore;
using TgBitGetBot.DataAccess;
using TgBitGetBot.Domain.MappingProfiles;
using TgBitGetBot.Infrastructure.Services;
using TgBitGetBot.Infrastructure.TelegramMessageListener;
using TgBitGetBot.DataAccess.Repos;
using TgBitGetBot.Infrastructure.CommandRouter;
using TgBitGetBot.Infrastructure.Workers;
using TgBitGetBot.Domain.Confgis;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Application.CommandRouter.Interface;
using TgBitGetBot.Application.TelegramMessageListener.Interface;
using TgBitGetBot.DataAccess.Repos.Interfaces;
using TgBitGetBot.Application.Factories.Interface;
using TgBitGetBot.Infrastructure.Factories;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Infrastructure.Commands;
using Microsoft.Extensions.Http;
using TgBitGetBot.Domain.Consts;

var builder = new ConfigurationBuilder();
BuildConfig(builder);
SetupLogger(builder);

var configuration = builder.Build();

var host = Host.CreateDefaultBuilder()
	.ConfigureServices((context, services) =>
	{
		services.Configure<EasyCacheConfigs>(configuration.GetSection("easycaching"));

		services.AddEasyCaching(options =>
		{
			//use memory cache
			options.UseInMemory(context.Configuration);
		});

		services.AddAutoMapper(typeof(TickerDtoToModelProfile), typeof(DtoToEntitesProfile));
		services.AddDbContext<ApplicationDbContext>(opt =>
			opt.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

		services.AddTransient<ITickerService, TickerService>();
		services.AddTransient<IUserService, UserService>();
		services.AddTransient<IUserRepository, UserRepository>();
		services.AddTransient<IUserStateRepository, UserStateRepository>();
		services.AddTransient<ICommandRouter, CommandRouter>();
		services.AddTransient<IUserApiInfoService, UserApiInfoService>();
		services.AddTransient<IUserApiInfoRepository, UserApiInfoRepository>();
		services.AddSingleton<ITelegramMessageListener, TelegramMessageListener>();
		services.AddTransient<ICommand, RegisterUserCommand>();
		services.AddTransient<ICommand, UnRegisterUserCommand>();
		services.AddTransient<ICommand, RegisterUserCommand>();
		services.AddTransient<ICommand, GetTopTickersByDepthCommand>();

		services.AddHttpClient(HttpClientConstNames.BitGetApiName, client =>
		{
			client.BaseAddress = new Uri(ApiRouteConsts.ApiRoute);
		});

		services.AddHostedService<TopTickersWorker>();
	})
	.UseSerilog()
	.Build();

//using (var scope = host.Services.CreateScope())
//{
//	var services = scope.ServiceProvider;

//	var context = services.GetRequiredService<ApplicationDbContext>();
//	if (context.Database.GetPendingMigrations().Any())
//	{
//		context.Database.Migrate();
//	}
//}

await host.StartAsync().ConfigureAwait(false);

var telegramMessageListener = host.Services.GetRequiredService<ITelegramMessageListener>();

await telegramMessageListener.StartListening();

await host.WaitForShutdownAsync().ConfigureAwait(false);


static void BuildConfig(IConfigurationBuilder builder)
{
	builder.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		.AddJsonFile($"appsettings{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
		.AddEnvironmentVariables();
}

static void SetupLogger(IConfigurationBuilder builder)
{
	Log.Logger = new LoggerConfiguration()
		.ReadFrom.Configuration(builder.Build())
		.Enrich.FromLogContext()
		.WriteTo.Console()
		.CreateLogger();
}