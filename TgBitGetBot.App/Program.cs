using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TgBitGetBot.DataAccess;
using TgBitGetBot.Domain.MappingProfiles;
using TgBitGetBot.Infrastructure.Services;
using TgBitGetBot.Infrastructure.Services.Interfaces;
using TgBitGetBot.Infrastructure.TelegramMessageListener;
using TgBitGetBot.DataAccess.Repos.Interfaces;
using TgBitGetBot.DataAccess.Repos;
using TgBitGetBot.Infrastructure.CommandRouter;

var builder = new ConfigurationBuilder();
BuildConfig(builder);
SetupLogger(builder);

var configuration = builder.Build();

var host = Host.CreateDefaultBuilder()
	.ConfigureServices((context, services) =>
	{
		services.AddAutoMapper(typeof(TickerDtoToModelProfile), typeof(DtoToEntitesProfile));
		services.AddDbContext<ApplicationDbContext>(opt =>
			opt.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
		services.AddTransient<ITelegramMessageListener, TelegramMessageListener>();
		services.AddTransient<ITickerService, TickerService>();
		services.AddTransient<IUserService, UserService>();
		services.AddTransient<IUserRepository, UserRepository>();
		services.AddTransient<ICommandRouter, CommandRouter>();

	})
	.UseSerilog()
	.Build();

var svc = ActivatorUtilities.CreateInstance<TelegramMessageListener>(host.Services);
await svc.StartListening();


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