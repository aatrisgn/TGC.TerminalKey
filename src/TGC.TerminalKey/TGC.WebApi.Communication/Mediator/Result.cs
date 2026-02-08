using System.Net;

namespace TGC.WebApi.Communication.Mediator;

public interface IResult<out T> where T : IMediatorResponse
{
	bool IsSuccess { get; }
	string? Error { get; }
	HttpStatusCode? ErrorType { get; }
	T? Value { get; }
}

public class Result<T> : IResult<T> where T : IMediatorResponse
{
	public bool IsSuccess { get; }
	public string? Error { get; }
	public HttpStatusCode? ErrorType { get; }
	public T? Value { get; }

	private Result(bool isSuccess, T? value, string? error)
	{
		IsSuccess = isSuccess;
		Value = value;
		Error = error;
	} 
	
	public static Result<T> Success(T value) => new Result<T>(true, value, null);
	public static Result<T> Failure(string error) => new Result<T>(false, default, error);
}