using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Abstractions;

public interface ICommandHandler
{
	public Task<ICommandResponse> Handle<TCommand>(TCommand command) where TCommand : ICommand;
	public bool Accepts(ICommand command);
}