using Microsoft.AspNetCore.Http;
using PurchaseManager.ClientHttp.Abstraction;
using PurchaseManager.Shared.DTO;
using System.Globalization;
using System.Net.Http.Json;

namespace PurchaseManager.ClientHttp;

public class ClientHttp(HttpClient httpClient) : IClientHttp
{
	#region Order
	public async Task<string?> CreateOrder(CreateOrderDto orderDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PostAsync($"Order/CreateOrder", JsonContent.Create(orderDto), cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
	}
	public Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	public async  Task<List<ReadOrderDto>> GetAllOrdersBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "codiceFiscale", supplierId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/Soggetto/ReadAllOrder{queryString}", cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<List<ReadOrderDto>>(cancellationToken: cancellationToken) ?? new List<ReadOrderDto>();
	}
	public async  Task<ReadOrderDto?> GetOrderAsync(int orderId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "orderId", orderId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/Order/ReadOrder{queryString}", cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<ReadOrderDto?>(cancellationToken: cancellationToken);
	}
	#endregion

	#region Product 
	public async  Task<string?> CreateProduct(IEnumerable<CreateProductDto> payload, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PostAsync($"Product/CreateListOfProducts", JsonContent.Create(payload), cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
	}
	public Task DeleteProduct(int productId, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	public async  Task<ReadProductDto?> GetProductListOfSupplier(int SupplierId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "supplierId", SupplierId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/Product/ReadProductListOfSupplier{queryString}", cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<ReadProductDto?>(cancellationToken: cancellationToken);
	}
	public Task UpdateProductList(UpdateProductDto productDto, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	#endregion

	#region Supplier
	public async Task<string?> CreateSupplier(CreateSupplierDto supplierDto, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PostAsync($"Order/CreateOrder", JsonContent.Create(supplierDto), cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
	}
	public Task DeleteSupplier(int supplierId, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	public async  Task<ReadSupplierDto?> GetSupplier(int supplierId, CancellationToken cancellationToken = default)
	{
		var queryString = QueryString.Create(new Dictionary<string, string?>() {
			{ "orderId", supplierId.ToString(CultureInfo.InvariantCulture) }
		});
		var response = await httpClient.GetAsync($"/Order/ReadOrder{queryString}", cancellationToken);
		return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<ReadSupplierDto?>(cancellationToken: cancellationToken);
	}
	public Task UpdateSupplier(UpdateSupplierDto supplierDto, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	#endregion
}
