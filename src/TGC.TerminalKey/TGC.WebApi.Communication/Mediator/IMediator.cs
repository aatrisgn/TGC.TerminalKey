using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Abstractions;

public interface IMediator
{
	public Task<T2> HandleCommandAsync<T1, T2>(T1 command) where T1 : ICommand where T2 :  ICommandResponse;
	public Task<T2> HandleQueryAsync<T1, T2>(T1 command) where T1 : IQuery where T2 : IQueryResponse;
}