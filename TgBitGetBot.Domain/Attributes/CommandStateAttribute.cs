using TgBitGetBot.Domain.Enums;

namespace TgBitGetBot.Domain.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class CommandStateAttribute : Attribute
	{
		public TelegramDialogState Key { get; }

		public CommandStateAttribute(TelegramDialogState state)
		{
			Key = state;
		}

	}
}
