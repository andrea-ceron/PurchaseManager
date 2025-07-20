

namespace PurchaseManager.Business;

public class ExceptionHandler : Exception
{
	public int StatusCode { get; }
	public new string? Message { get; } 
	public object? InvolvedElement { get; }

	public ExceptionHandler(string message, int statusCode = 400)
		: base(message)
	{
		StatusCode = statusCode;
		Message = message;
	}

	public ExceptionHandler(string message, object elem, int statusCode = 400)
		: base(message)
	{
		StatusCode = statusCode;
		Message = message;
		InvolvedElement = elem;
	}
}
