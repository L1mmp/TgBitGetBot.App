using EasyCaching.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.CommandRouter.Interface;
using TgBitGetBot.Application.Factories.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Infrastructure.Commands;
using TgBitGetBot.Infrastructure.Factories;

namespace TgBitGetBot.Infrastructure.CommandRouter
{
	public class CommandRouter : ICommandRouter
	{
		private readonly IEnumerable<ICommand> _commandsImpls;

		//private Dictionary<string, Type> _commands = new()
		//{
		//	{"/register",  },
		//	{"/unregister", typeof(UnRegisterUserCommand) },
		//	{"/top5", typeof(GetTopTickersByDepthCommand) },
		//	{"/registerNewUserApi", typeof(RegisterUserApiCommand) }
		//};
		public CommandRouter(IEnumerable<ICommand> commandsImpls)
		{
			_commandsImpls = commandsImpls;
		}

		public async Task ExecuteCommand(Message message, ITelegramBotClient botClient)
		{
			if (ValidateMessage(message))
			{
				switch (message.Text)
				{
					case "/register":
						await _commandsImpls.FirstOrDefault(x => x.GetType() == typeof(RegisterUserCommand)).Execute(message, botClient);
						break;
					case "/unregister":
						await _commandsImpls.FirstOrDefault(x => x.GetType() == typeof(UnRegisterUserCommand)).Execute(message, botClient);
						break;
					case "/top5":
						await _commandsImpls.FirstOrDefault(x => x.GetType() == typeof(GetTopTickersByDepthCommand)).Execute(message, botClient);
						break;
					case "/registerNewUserApi":
						await _commandsImpls.FirstOrDefault(x => x.GetType() == typeof(RegisterUserApiCommand)).Execute(message, botClient);
						break;
				}
			}

			await Task.CompletedTask;
		}

		private static bool ValidateMessage(Message message)
		{
			var isNotEmpty = !string.IsNullOrWhiteSpace(message.Text);
			var isMesssage = message.Text is { } messageText;

			return isNotEmpty || isMesssage;
		}
	}
}
