using PurchaseManager.Business;
using System.Text.Json;

namespace PurchaseManager.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly ILogger<ExceptionMiddleware> _logger = logger;

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context); 
		}
		catch (ExceptionHandler ex)
		{
			_logger.LogError(ex, "Errore controllato");
			context.Response.StatusCode = ex.StatusCode;
			context.Response.ContentType = "application/json";

			var result = JsonSerializer.Serialize(new
			{
				error = ex.Message
			});

			await context.Response.WriteAsync(result);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Errore generico non gestito");
			context.Response.StatusCode = 500;
			context.Response.ContentType = "application/json";

			var result = JsonSerializer.Serialize(new
			{
				error = "Errore interno del server"
			});

			await context.Response.WriteAsync(result);
		}
	}
}
