using Microsoft.AspNetCore.Mvc;
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
	public async Task<ActionResult<ReadSupplierDto>> CreateSupplier(CreateSupplierDto payload)
	{
		var supplierCreated = await _business.CreateSupplierAsync(payload);
		return Ok(supplierCreated);
	}

	[HttpGet(Name = "ReadSupplierReadSupplier")]
	public async Task<ActionResult<ReadSupplierDto>> ReadSupplier(int supplierId)
	{
		ReadSupplierDto? Supplier = await _business.GetSupplierAsync(supplierId);
		return Ok(Supplier);
	}

	[HttpPut(Name = "UpdateSupplier")]
	public async Task<ActionResult<ReadSupplierDto>> UpdateSupplier(UpdateSupplierDto payload)
	{
		var SupplierUpdated = await _business.UpdateSupplierAsync(payload);
		return Ok(SupplierUpdated);
	}

	[HttpDelete(Name = "DeleteSupplier")]
	public async Task<ActionResult<string>> DeleteSupplier(int SupplierId)
	{
		await _business.DeleteSupplierAsync(SupplierId);
		return Ok("Eliminazione del Fornitore Eseguita con successo");
	}

}
