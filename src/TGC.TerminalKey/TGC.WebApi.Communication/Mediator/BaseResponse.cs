using System.Net;

namespace TGC.WebApi.Communication.Mediator;

public abstract class BaseResponse : ICommandResponse, IQueryResponse
{
	public HttpStatusCode StatusCode { get; set; }
	public string? ErrorResponse { get; set; }
}