

namespace PurchaseManager.Business;

    public class ExceptionHandler : Exception
    {
	public int StatusCode { get; }
	public string Message { get; }
	public Object? InvolvedElement { get; }

	public ExceptionHandler(string message, int statusCode = 400)
		: base(message)
	{
		StatusCode = statusCode;
		Message = message;
	}

	public ExceptionHandler(string message, Object elem, int statusCode = 400)
		:base(message)
	{
		StatusCode = statusCode;
		Message = message;
		InvolvedElement = elem;

	}
}
