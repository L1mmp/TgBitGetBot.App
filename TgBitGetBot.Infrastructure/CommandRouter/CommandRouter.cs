using Microsoft.Extensions.Logging;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBitGetBot.Application.Command.Interface;
using TgBitGetBot.Application.CommandRouter.Interface;
using TgBitGetBot.Application.Services.Interfaces;
using TgBitGetBot.Domain.Attributes;
using TgBitGetBot.Domain.Consts;
using TgBitGetBot.Domain.Entities;
using TgBitGetBot.Domain.Enums;
using TgBitGetBot.Infrastructure.Commands;

namespace TgBitGetBot.Infrastructure.CommandRouter
{
	public class CommandRouter : ICommandRouter
	{
		private readonly IEnumerable<IKeyCommand> _keyCommandsImpls;
		private readonly IEnumerable<IStateCommand> _stateCommandsImpls;
		private readonly Dictionary<string, IKeyCommand> _commandKeyMap = new()!;
		private readonly Dictionary<TelegramDialogState, IStateCommand> _commandStateMap = new()!;
		private readonly IUserStateService _userStateService;
		private readonly ILogger _logger;
		private readonly IUserService _userService;

		public CommandRouter(
			IEnumerable<IKeyCommand> keyCommandsImpls,
			IEnumerable<IStateCommand> stateCommandsImpls,
			IUserStateService userStateService,
			ILoggerFactory loggerFactory,
			IUserService userService)
		{
			_keyCommandsImpls = keyCommandsImpls;
			_stateCommandsImpls = stateCommandsImpls;
			_userStateService = userStateService;


			foreach (var comma in keyCommandsImpls)
			{
				var commaType = comma.GetType();

				var atrribute = commaType.GetCustomAttribute<CommandKeyAttribute>();

				if (atrribute is null)
				{
					continue;
				}

				_commandKeyMap.Add(atrribute!.Key, comma);
			}

			foreach (var comma in stateCommandsImpls)
			{
				var commaType = comma.GetType();

				var atrribute = commaType.GetCustomAttribute<CommandStateAttribute>();

				if (atrribute is null)
				{
					continue;
				}

				_commandStateMap.Add(atrribute!.Key, comma);
			}

			_logger = loggerFactory.CreateLogger<CommandRouter>();
			_userService = userService;
		}

		public async Task ExecuteCommand(Message message, ITelegramBotClient botClient)
		{
			try
			{
				var user = await _userService.GetUserByTelegramId(message.Chat.Id);

				if (user == null && message.Text != CommandNames.RegisterUserCommandName)
				{
					await _keyCommandsImpls.FirstOrDefault(x => x.GetType() == typeof(NeedToRegisterUserCommand))!.ExecuteAsync(message, botClient);
					return;
				}

				var userState = await _userStateService.GetCurrentStateOfUser(message.Chat.Id);

				if (userState is default(UserState) || message.Text == CommandNames.ResetCommandName)
				{
					await _userStateService.UpdateUserState(message.Chat.Id, TelegramDialogState.DefaultSatate);
				}

				if (ValidateMessage(message))
				{

					if (userState!.State != TelegramDialogState.DefaultSatate)
					{
						if (!_commandStateMap.TryGetValue(userState!.State, out var command))
						{
							await _keyCommandsImpls.FirstOrDefault(x => x.GetType() == typeof(DefaultCommand))!.ExecuteAsync(message, botClient);
						}
						else
						{
							await command.ExecuteAsync(message, botClient);
						}
					}
					else
					{

						if (!_commandKeyMap.TryGetValue(message.Text!, out var command))
						{
							await _keyCommandsImpls.FirstOrDefault(x => x.GetType() == typeof(DefaultCommand))!.ExecuteAsync(message, botClient);
						}
						else
						{
							await command.ExecuteAsync(message, botClient);
						}
					}
				}


			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw;
			}
		}

		private static bool ValidateMessage(Message message)
		{
			var isNotEmpty = !string.IsNullOrWhiteSpace(message.Text);
			var isMesssage = message.Text is { } messageText;

			return isNotEmpty || isMesssage;
		}
	}
}
