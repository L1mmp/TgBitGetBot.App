using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Infrastructure.Commands;
using TgBitGetBot.Infrastructure.Services.Interfaces;

namespace TgBitGetBot.Infrastructure.CommandRouter
{
	public class CommandRouter : ICommandRouter
	{
		private readonly IUserService _userService;
		private readonly ITickerService _tickerService;
		public CommandRouter(IUserService userService, ITickerService tickerService)
		{
			_userService = userService;
			_tickerService = tickerService;
		}

		public Task ExecuteCommand(Message message, ITelegramBotClient botClient)
		{
			if (ValidateMessage(message))
			{
				switch (message.Text)
				{
					case "/register": return new RegisterUserCommand(_userService, message, botClient).Execute();
					case "/unregister": return new UnRegisterUserCommand(_userService, message, botClient).Execute();
					case "/top5": return new GetTopTickersByDepthCommand(_tickerService, message, botClient).Execute();
				}
			}

			return Task.CompletedTask;
		}

		private static bool ValidateMessage(Message message)
		{
			var isNotEmpty = !string.IsNullOrWhiteSpace(message.Text);
			var isMesssage = message.Text is { } messageText;

			return isNotEmpty || isMesssage;
		}
	}
}
