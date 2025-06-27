using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Business;
using PurchaseManager.Shared.DTO;

namespace PurchaseManager.Api.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class OrderController(IBusiness business, ILogger<SupplierController> logger) : Controller
    {
	private readonly IBusiness _business = business;
	private readonly ILogger<SupplierController> _logger = logger;

	[HttpPost(Name = "CreateOrder")]
	public async Task<ActionResult> CreateOrderAsync(CreateOrderDto payload)
	{
		await _business.CreateOrderAsync(payload);
		return Ok();
	}

	[HttpGet(Name = "ReadOrder")]
	public async Task<ActionResult> GetOrderAsync(int OrderId)
	{
		ReadOrderDto? Order = await _business.GetOrderByIdAsync(OrderId);
		return new JsonResult(Order);
	}

	[HttpGet(Name = "ReadAllOrder")]
	public async Task<ActionResult<List<ReadOrderDto>>> GetAllOrdersBySupplierIdAsync(int supplierId)
	{
		List<ReadOrderDto>? OrderList = await _business.GetAllOrdersBySupplierIdAsync(supplierId);
		if(OrderList == null || OrderList.Count == 0)
			return NotFound("No orders found for this supplier");
		return Ok(OrderList);
	}

	[HttpDelete(Name = "DeleteOrder")]
	public async Task<ActionResult> DeleteOrderAsync(int OrderId)
	{
		await _business.DeleteOrderAsync(OrderId);
		return Ok();
	}
}
