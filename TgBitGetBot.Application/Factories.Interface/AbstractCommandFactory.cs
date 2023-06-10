using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using TgBitGetBot.Application.Command.Interface;

namespace TgBitGetBot.Application.Factories.Interface
{
	public abstract class AbstractCommandFactory
	{
		public readonly IServiceProvider _serviceProvider;
		public readonly Message _message;
		public readonly ITelegramBotClient _botClient;
		public AbstractCommandFactory(IServiceProvider serviceProvider, Message message, ITelegramBotClient botClient) 
		{ 
			_serviceProvider = serviceProvider;
			_message = message;
			_botClient = botClient;
		}
	}
}
