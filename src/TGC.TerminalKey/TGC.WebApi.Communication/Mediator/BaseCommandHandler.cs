namespace TGC.HomeAutomation.Application.Abstractions;

public abstract class BaseCommandHandler<TCommand> where TCommand : class, ICommand
{
	public virtual bool Accepts(ICommand query) => query is TCommand;
	protected TCommand GetTypedCommand(ICommand command) => command as TCommand;
}