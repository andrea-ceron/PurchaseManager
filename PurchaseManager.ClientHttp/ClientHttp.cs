using Microsoft.AspNetCore.Http;
using PurchaseManager.ClientHttp.Abstraction;
using PurchaseManager.Shared.DTO;
using System.Globalization;
using System.Net.Http.Json;

namespace PurchaseManager.ClientHttp;

public class ClientHttp(HttpClient httpClient) : IClientHttp
{
	#region SupplierOrder
	public async Task<string?> CreateSupplierOrder(CreateSupplierOrderDto SupplierOrderDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PostAsync($"SupplierOrder/CreateSupplierOrder", JsonContent.Create(SupplierOrderDto), cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
	}
	public async  Task<List<ReadSupplierOrderDto>> GetAllSupplierOrdersBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "supplierId", supplierId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/SupplierOrder/ReadAllSupplierOrder{queryString}", cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<List<ReadSupplierOrderDto>>(cancellationToken: cancellationToken) ?? new List<ReadSupplierOrderDto>();
	}
	public async  Task<ReadSupplierOrderDto?> GetSupplierOrderAsync(int SupplierOrderId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "SupplierOrderId", SupplierOrderId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/SupplierOrder/ReadSupplierOrder{queryString}", cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<ReadSupplierOrderDto?>(cancellationToken: cancellationToken);
	}
	#endregion

	#region RawMaterial 
	public async  Task<string?> CreateRawMaterial(IEnumerable<CreateRawMaterialDto> payload, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PostAsync($"RawMaterial/CreateListOfRawMaterials", JsonContent.Create(payload), cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
	}
	public async  Task<string?> DeleteRawMaterial(int RawMaterialId, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.DeleteAsync($"/RawMaterial/DeleteRawMaterial?RawMaterialId={RawMaterialId}", cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
	}
	public async  Task<List<ReadRawMaterialDto>?> GetRawMaterialListOfSupplier(int SupplierId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "supplierId", SupplierId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/RawMaterial/ReadRawMaterialListOfSupplier{queryString}", cancellationToken);
		return await response.EnsureSuccessStatusCode()
								 .Content
								 .ReadFromJsonAsync<List<ReadRawMaterialDto>>(cancellationToken: cancellationToken)
								 ?? new List<ReadRawMaterialDto>();
	}
	public async  Task<string?> UpdateRawMaterialList(UpdateRawMaterialDto RawMaterialDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PutAsync($"/RawMaterial/UpdateRawMaterial", JsonContent.Create(RawMaterialDto), cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken	);
	}
	#endregion

	#region Supplier
	public async Task<string?> CreateSupplier(CreateSupplierDto supplierDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PostAsync($"Supplier/CreateSupplier", JsonContent.Create(supplierDto), cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
	}
	public async Task<string?> DeleteSupplier(int supplierId, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.DeleteAsync($"/Supplier/DeleteSupplier?supplierId={supplierId}", cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
	}
	public async  Task<ReadSupplierDto?> GetSupplier(int supplierId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "supplierId", supplierId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/Supplier/ReadSupplier{queryString}", cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<ReadSupplierDto?>(cancellationToken: cancellationToken);
	}
	public async Task<string?> UpdateSupplier(UpdateSupplierDto supplierDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PutAsync($"/Supplier/UpdateSupplier", JsonContent.Create(supplierDto), cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
	}
	#endregion
}
