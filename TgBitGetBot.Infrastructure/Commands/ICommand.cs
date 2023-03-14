namespace TgBitGetBot.Infrastructure.Commands;

public interface ICommand
{
	public Task Execute();
	public Task UnExecute();

}