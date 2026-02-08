using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace TGC.WebApi.Communication;

public class ApiResult<T>: ApiResult where T : class?
{
	private T? result;
	
	private ApiResult(HttpStatusCode statusCode, T result) : base(statusCode)
	{
		this.statusCode = statusCode;
		this.result = result;
	}
	
	public static ApiResult<T?> AsOk(T result)
	{
		return new ApiResult<T?>(HttpStatusCode.OK, result);
	}
	
	public static ApiResult<T?> AsNotFound()
	{
		return new ApiResult<T?>(HttpStatusCode.NotFound, null);
	}

	public T Result()
	{
		return result ?? throw new ArgumentNullException("Result was unexpected null.");
	}
	
	public override IActionResult ToActionResult()
	{
		return new OkObjectResult(result);
	}
}

public class ApiResult
{
	protected HttpStatusCode statusCode;
	
	protected ApiResult(HttpStatusCode statusCode)
	{
		this.statusCode = statusCode;
	}
	
	public static ApiResult FromStatusCode(HttpStatusCode statusCode)
	{
		return new ApiResult(statusCode);
	}

	public static ApiResult AsNotFound()
	{
		return new ApiResult(HttpStatusCode.NotFound);
	}
	
	public static ApiResult AsUnauthorized()
	{
		return new ApiResult(HttpStatusCode.Unauthorized);
	}
	
	public static ApiResult AsForbidden()
	{
		return new ApiResult(HttpStatusCode.Forbidden);
	}
	
	public virtual IActionResult ToActionResult()
	{
		switch (statusCode)
		{
			case HttpStatusCode.NotFound:
				return new NotFoundResult();
			case HttpStatusCode.OK:
				return new OkResult();
			default:
				throw new Exception("No HttpStatusCode were handled."); //Should be changed to another exception type later
		}
	}
}