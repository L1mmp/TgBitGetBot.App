namespace TgBitGetBot.Domain.Consts
{
	public record CommandNames
	{
		public const string DefaultCommandName = "default";
		public const string NeedToregisterUser = "needToRegisterUser";
		public const string GetTopTickersByDepthCommandName = "/top5";
		public const string RegisterUserApiCommandName = "/registerNewUserApi";
		public const string RegisterUserCommandName = "/register";
		public const string UnRegisterUserCommandName = "/unregister";
		public const string ResetCommandName = "/reset";

	}
}
