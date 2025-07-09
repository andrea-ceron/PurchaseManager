using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Shared.DTO;

namespace PurchaseManager.Api.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class SupplierOrderController(IBusiness business, ILogger<SupplierOrderController> logger) : Controller
    {
	private readonly IBusiness _business = business;
	private readonly ILogger<SupplierOrderController> _logger = logger;

	[HttpPost(Name = "CreateSupplierOrder")]
	public async Task<ActionResult<List<UpdateRawMaterialQuantity>>> CreateSupplierOrderAsync(CreateSupplierOrderDto payload)
	{
		var res = await _business.CreateSupplierOrderAsync(payload);
		if(res == null || res.Count == 0)
		{
			_logger.LogWarning("Nessun prodotto creato.");
			return BadRequest("Nessun prodotto creato.");
		}
		return Ok(res);
	}

	[HttpGet(Name = "ReadSupplierOrder")]
	public async Task<ActionResult> GetrSupplierOrderAsync(int SupplierSupplierOrderId)
	{
		ReadSupplierOrderDto? SupplierOrder = await _business.GetSupplierOrderByIdAsync(SupplierSupplierOrderId);
		return Ok(new JsonResult(SupplierOrder));
	}

	[HttpGet(Name = "ReadAllSupplierOrder")]
	public async Task<ActionResult<List<ReadSupplierOrderDto>>> GetAllSupplierOrdersBySupplierIdAsync(int supplierId)
	{
		List<ReadSupplierOrderDto>? SupplierOrderList = await _business.GetAllSupplierOrdersBySupplierIdAsync(supplierId);
		if(SupplierOrderList == null || SupplierOrderList.Count == 0)
			return NotFound("No SupplierOrders found for this supplier");
		return Ok(SupplierOrderList);
	}

}
