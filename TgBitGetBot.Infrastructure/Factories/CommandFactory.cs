using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.Factories.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Infrastructure.Commands;
using TgBitGetBot.Infrastructure.Services;

namespace TgBitGetBot.Infrastructure.Factories
{
	public class CommandFactory : AbstractCommandFactory
	{
		public CommandFactory(
			IServiceProvider serviceProvider,
			Message message,
			ITelegramBotClient botClient)
			:
			base(
				serviceProvider,
				message,
				botClient)
		{ }
	}
}
