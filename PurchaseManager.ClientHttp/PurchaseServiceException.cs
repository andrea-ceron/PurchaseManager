namespace PurchaseManager.ClientHttp;

public class PurchaseServiceException : Exception
{
	public int StatusCode { get; }
	public string? ResponseContent { get; }

	public PurchaseServiceException(int statusCode, string? responseContent)
		: base($"Chiamata a PurchaseService fallita con StatusCode {statusCode}")
	{
		StatusCode = statusCode;
		ResponseContent = responseContent;
	}
}
