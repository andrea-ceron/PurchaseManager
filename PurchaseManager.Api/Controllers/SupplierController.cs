using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PurchaseManager.Business;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Shared.DTO;

namespace PurchaseManager.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SupplierController(IBusiness business, ILogger<SupplierController> logger) : Controller
{
	private readonly IBusiness _business = business;
	private readonly ILogger<SupplierController> _logger = logger;



	[HttpPost(Name = "CreateSupplier")]
	public async Task<ActionResult> CreateSupplier(CreateSupplierDto payload)
	{
		await _business.CreateSupplierAsync(payload);
		return Ok();
	}

	[HttpGet(Name = "ReadSupplier")]
	public async Task<ActionResult<ReadSupplierDto>> GetSupplier(int SupplierId)
	{
		ReadSupplierDto? Supplier = await _business.GetSupplierAsync(SupplierId);
		if (Supplier == null) return NotFound("Supplier non trovato");
		return Ok(Supplier);
	}

	[HttpPut(Name = "UpdateSupplier")]
	public async Task<ActionResult> UpdateSupplier(UpdateSupplierDto payload)
	{
		await _business.UpdateSupplierAsync(payload);
		return Ok();
	}

	[HttpDelete(Name = "DeleteSupplier")]
	public async Task<ActionResult> DeleteSupplier(int SupplierId)
	{
		await _business.DeleteSupplierAsync(SupplierId);
		return Ok();
	}

}
