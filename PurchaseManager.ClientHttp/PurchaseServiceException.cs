namespace PurchaseManager.ClientHttp;

public class PurchaseServiceException(int statusCode, string? responseContent) : Exception($"Chiamata a PurchaseService fallita con StatusCode {statusCode}")
{
	public int StatusCode { get; } = statusCode;
	public string? ResponseContent { get; } = responseContent;
}
