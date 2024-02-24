using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TgBitGetBot.App.Extensions;
using TgBitGetBot.Application.CommandRouter.Interface;
using TgBitGetBot.Application.TelegramMessageListener.Interface;
using TgBitGetBot.DataAccess;
using TgBitGetBot.Domain.Confgis;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.MappingProfiles;
using TgBitGetBot.Infrastructure.CommandRouter;
using TgBitGetBot.Infrastructure.TelegramMessageListener;
using TgBitGetBot.Infrastructure.Workers;


var configBuilder = new ConfigurationBuilder();
BuildConfig(configBuilder);
SetupLogger(configBuilder);

var configuration = configBuilder.Build();

var host = Host.CreateDefaultBuilder()
	.ConfigureServices((context, services) =>
	{
		services.Configure<EasyCacheConfigs>(configuration.GetSection("easycaching"));

		services.AddEasyCaching(options =>
		{
			options.UseInMemory(context.Configuration);
		});

		services.AddAutoMapper(typeof(TickerDtoToModelProfile), typeof(DtoToEntitesProfile));

		services.AddDbContext<ApplicationDbContext>(opt =>
		{
			opt.UseNpgsql(context.Configuration.GetConnectionString("Postgres"));
		});


		services.AddRepositories();
		services.AddServices();

		services.AddTransient<ICommandRouter, CommandRouter>();
		services.AddTransient<ITelegramMessageListener, TelegramMessageListener>();

		services.AddTelegramBotClient();

		services.AddKeyCommands();
		services.AddStateCommands();


		services.AddHttpClient(HttpClientConstNames.BitGetApiName, client =>
		{
			client.BaseAddress = new Uri(ApiRouteConsts.ApiRoute);
		});

		services.AddHostedService<TopTickersWorker>();
	})
	.UseSerilog()
	.Build()
	.MigrateDatabase<ApplicationDbContext>();

await StartApplication(host);

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

static async Task StartApplication(IHost host)
{
	await host.StartAsync();

	var telegramMessageListener = host.Services.GetRequiredService<ITelegramMessageListener>();

	await telegramMessageListener.StartListening();
	await host.WaitForShutdownAsync();
}