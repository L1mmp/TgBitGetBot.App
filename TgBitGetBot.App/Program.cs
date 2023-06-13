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
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Infrastructure.Commands;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.App.Extensions;

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
		//services.AddDbContext<ApplicationDbContext>(opt =>
		//{
		//	opt.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"));
		//	//opt.UseSqlServer(context.Configuration.GetConnectionString("Docker"));
		//});
		services.AddDbContext<ApplicationDbContext>(opt =>
		{
			opt.UseNpgsql(context.Configuration.GetConnectionString("Postgres"));
		});

		services.AddTransient<ITickerService, TickerService>();
		services.AddTransient<IUserService, UserService>();
		services.AddTransient<IUserApiInfoService, UserApiInfoService>();
		services.AddTransient<IUserStateService, UserStateService>();

		services.AddTransient<IUserRepository, UserRepository>();
		services.AddTransient<IUserStateRepository, UserStateRepository>();
		services.AddScoped<ICommandRouter, CommandRouter>();
		services.AddTransient<IUserApiInfoRepository, UserApiInfoRepository>();

		services.AddSingleton<ITelegramMessageListener, TelegramMessageListener>();
		services.AddTelegramBotClient();

		services.AddTransient<IKeyCommand, RegisterUserCommand>();
		services.AddTransient<IKeyCommand, UnRegisterUserCommand>();
		services.AddTransient<IKeyCommand, RegisterUserApiCommand>();
		services.AddTransient<IKeyCommand, GetTopTickersByDepthCommand>();
		services.AddTransient<IKeyCommand, DefaultCommand>();
		services.AddTransient<IKeyCommand, ResetStateCommand>();

		services.AddTransient<IStateCommand, UpdateUserApiTokenCommand>();
		services.AddTransient<IStateCommand, UpdateUserApiPassphraceCommand>();

		services.AddHttpClient(HttpClientConstNames.BitGetApiName, client =>
		{
			client.BaseAddress = new Uri(ApiRouteConsts.ApiRoute);
		});

		services.AddHostedService<TopTickersWorker>();
	})
	.UseSerilog()
	.Build()
	.MigrateDatabase<ApplicationDbContext>();



await host.StartAsync()
	.ConfigureAwait(false);

var telegramMessageListener = host.Services.GetRequiredService<ITelegramMessageListener>();

await telegramMessageListener.StartListening();

await host.WaitForShutdownAsync()
	.ConfigureAwait(false);




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