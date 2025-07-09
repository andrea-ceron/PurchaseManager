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



	[HttpPost(Name = "CreateListOfRawMaterials")]
	public async Task<ActionResult> CreateRawMaterial(IEnumerable<CreateRawMaterialDto> payload)
	{
		await _business.CreateListOfRawMaterialsAsync(payload);
		return Ok();
	}

	[HttpGet(Name = "ReadRawMaterialById")]
	public async Task<ActionResult<ReadRawMaterialDto>> GetRawMaterialById(int rawMaterialId)
	{
		ReadRawMaterialDto? RawMaterialDto = await _business.GetRawMaterialById(rawMaterialId);
		if (RawMaterialDto == null) return NotFound("RawMaterial non trovato");
		return Ok(RawMaterialDto);
	}

	[HttpPut(Name = "UpdateRawMaterialList")]
	public async Task<ActionResult> UpdateRawMaterialList(UpdateRawMaterialDto payload)
	{
		await _business.UpdateRawMaterialAsync(payload);
		return Ok();
	}

	[HttpDelete(Name = "DeleteRawMaterial")]
	public async Task<ActionResult> DeleteRawMaterial(int RawMaterialId)
	{
		await _business.DeleteRawMaterialAsync(RawMaterialId);
		return Ok();
	}


}
