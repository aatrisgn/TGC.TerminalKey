using System.Net;

namespace TGC.WebApi.Communication.Mediator;

public interface ICommandResponse : IMediatorResponse
{
	HttpStatusCode StatusCode { get; set; }
	string? ErrorResponse { get; set; }
}