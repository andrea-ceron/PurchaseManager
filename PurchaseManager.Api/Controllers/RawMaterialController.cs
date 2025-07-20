using Microsoft.AspNetCore.Mvc;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Shared.DTO;

namespace PurchaseManager.Api.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class RawMaterialController(IBusiness business, ILogger<RawMaterialController> logger) : Controller
{
	private readonly IBusiness _business = business;
	private readonly ILogger<RawMaterialController> _logger = logger;



	[HttpPost(Name = "CreateRawMaterial")]
	public async Task<ActionResult<IEnumerable<ReadRawMaterialDto>>> CreateListOfRawMaterials(IEnumerable<CreateRawMaterialDto> payload)
	{
		var ListOfRawMAterialsCreated = await _business.CreateListOfRawMaterialsAsync(payload);
		return Ok(ListOfRawMAterialsCreated);
	}

	[HttpGet(Name = "ReadRawMaterialById")]
	public async Task<ActionResult<ReadRawMaterialDto>> ReadRawMaterialById(int rawMaterialId)
	{
		ReadRawMaterialDto RawMaterialDto = await _business.GetRawMaterialById(rawMaterialId);
		return Ok(RawMaterialDto);
	}

	[HttpPut(Name = "UpdateRawMaterial")]
	public async Task<ActionResult<ReadRawMaterialDto>> UpdateRawMaterial(UpdateRawMaterialDto payload)
	{
		var UpdatedRawMaterial = await _business.UpdateRawMaterialAsync(payload);
		return Ok(UpdatedRawMaterial);
	}

	[HttpDelete(Name = "DeleteRawMaterial")]
	public async Task<ActionResult> DeleteRawMaterial(int RawMaterialId)
	{
		await _business.DeleteRawMaterialAsync(RawMaterialId);
		return Ok($"Eliminazione del RawMAterial con ID {RawMaterialId} eseguita con successo");
	}


}
