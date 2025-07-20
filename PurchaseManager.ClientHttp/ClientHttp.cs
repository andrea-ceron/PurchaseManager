using Microsoft.AspNetCore.Http;
using PurchaseManager.ClientHttp.Abstraction;
using PurchaseManager.Shared.DTO;
using System.Globalization;
using System.Net.Http.Json;
namespace PurchaseManager.ClientHttp;

public class PurchaseManagerClientHttp(HttpClient httpClient) : IPurchaseManagerClientHttp
{
	#region SupplierOrder
	public async Task<IEnumerable<UpdateRawMaterialQuantity>?> CreateSupplierOrder(CreateSupplierOrderDto SupplierOrderDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PostAsync($"SupplierOrder/CreateSupplierOrder", JsonContent.Create(SupplierOrderDto), cancellationToken);
		if (!response.IsSuccessStatusCode)
			throw new PurchaseServiceException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
		return await response.Content.ReadFromJsonAsync<List<UpdateRawMaterialQuantity>>(cancellationToken: cancellationToken);
	}
	public async  Task<List<ReadSupplierOrderDto>> GetAllSupplierOrdersBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "supplierId", supplierId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/SupplierOrder/ReadAllSupplierOrder{queryString}", cancellationToken);
		if(!response.IsSuccessStatusCode)
			throw new PurchaseServiceException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
		return await response.Content.ReadFromJsonAsync<List<ReadSupplierOrderDto>>(cancellationToken: cancellationToken) 
			?? throw new PurchaseServiceException((int)response.StatusCode, "Supplier order data is null.");
	}
	public async Task<ReadSupplierOrderDto> GetSupplierOrderAsync(int SupplierOrderId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "SupplierOrderId", SupplierOrderId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/SupplierOrder/ReadSupplierOrder{queryString}", cancellationToken);
		if (!response.IsSuccessStatusCode)
			throw new PurchaseServiceException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
		return await response.Content.ReadFromJsonAsync<ReadSupplierOrderDto>(cancellationToken: cancellationToken)
			   ?? throw new PurchaseServiceException((int)response.StatusCode, "Supplier order data is null.");
	}
	#endregion

	#region RawMaterial 
	public async  Task<IEnumerable<ReadRawMaterialDto>> CreateListOfRawMaterials(IEnumerable<CreateRawMaterialDto> payload, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PostAsync($"RawMaterial/CreateListOfRawMaterials", JsonContent.Create(payload), cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			throw new PurchaseServiceException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken));
		}
		return await response.Content.ReadFromJsonAsync<IEnumerable<ReadRawMaterialDto>>(cancellationToken: cancellationToken) 
			?? throw new PurchaseServiceException((int)response.StatusCode, "Raw material data is null.");
	}
	public async  Task<string?> DeleteRawMaterial(int RawMaterialId, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.DeleteAsync($"/RawMaterial/DeleteRawMaterial?RawMaterialId={RawMaterialId}", cancellationToken);
		var content = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
		if (!response.IsSuccessStatusCode)
			throw new PurchaseServiceException((int)response.StatusCode, content);
		return content;
	}
	public async  Task<IEnumerable<ReadRawMaterialDto>> GetRawMaterialListOfSupplier(int SupplierId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "supplierId", SupplierId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/RawMaterial/ReadRawMaterialListOfSupplier{queryString}", cancellationToken);
		if(!response.IsSuccessStatusCode)
			throw new PurchaseServiceException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
		return await response.Content.ReadFromJsonAsync<IEnumerable<ReadRawMaterialDto>>(cancellationToken: cancellationToken)
			?? throw new PurchaseServiceException((int)response.StatusCode, "Raw material data is null.");
	}
	public async  Task<ReadRawMaterialDto> UpdateRawMaterial(UpdateRawMaterialDto RawMaterialDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PutAsync($"/RawMaterial/UpdateRawMaterial", JsonContent.Create(RawMaterialDto), cancellationToken);
		if (!response.IsSuccessStatusCode)
			throw new PurchaseServiceException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken));
		return await response.Content.ReadFromJsonAsync<ReadRawMaterialDto>(cancellationToken: cancellationToken)
			?? throw new PurchaseServiceException((int)response.StatusCode, "Raw material data is null.");
	}
	#endregion

	#region Supplier
	public async Task<ReadSupplierDto> CreateSupplier(CreateSupplierDto supplierDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PostAsync($"Supplier/CreateSupplier", JsonContent.Create(supplierDto), cancellationToken);
		if (!response.IsSuccessStatusCode)
			throw new PurchaseServiceException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
		return await response.Content.ReadFromJsonAsync<ReadSupplierDto>(cancellationToken: cancellationToken) ?? throw new PurchaseServiceException((int)response.StatusCode, "Supplier data is null.");
	}
	public async Task<string?> DeleteSupplier(int supplierId, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.DeleteAsync($"/Supplier/DeleteSupplier?supplierId={supplierId}", cancellationToken);
		var content = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			throw new PurchaseServiceException((int)response.StatusCode, content);
		}
		return content;
	}

	public async Task<ReadSupplierDto> GetSupplier(int supplierId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "supplierId", supplierId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/Supplier/ReadSupplier{queryString}", cancellationToken);
		if (!response.IsSuccessStatusCode)
			throw new PurchaseServiceException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
		return await response.Content.ReadFromJsonAsync<ReadSupplierDto>(cancellationToken: cancellationToken) ?? throw new PurchaseServiceException((int)response.StatusCode, "Supplier data is null.");
	}

	public async Task<ReadSupplierDto> UpdateSupplier(UpdateSupplierDto supplierDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PutAsync($"/Supplier/UpdateSupplier", JsonContent.Create(supplierDto), cancellationToken);
		if (!response.IsSuccessStatusCode)
			throw new PurchaseServiceException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
		return await response.Content.ReadFromJsonAsync<ReadSupplierDto>(cancellationToken: cancellationToken) ?? throw new PurchaseServiceException((int)response.StatusCode, "Supplier data is null.");
	}
	#endregion
}
