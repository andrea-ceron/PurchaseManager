
using PurchaseManager.Shared.DTO;

namespace PurchaseManager.ClientHttp.Abstraction;

public interface IClientHttp
{
	Task<string?> CreateSupplier(CreateSupplierDto supplierDto, CancellationToken cancellationToken = default);
    Task<ReadSupplierDto?> GetSupplier(int supplierId, CancellationToken cancellationToken = default);
    Task UpdateSupplier(UpdateSupplierDto supplierDto, CancellationToken cancellationToken = default);
	Task DeleteSupplier(int supplierId, CancellationToken cancellationToken = default);

	Task<string?> CreateProduct(IEnumerable<CreateProductDto> payload, CancellationToken cancellationToken = default);
	Task<ReadProductDto?> GetProductListOfSupplier(int SupplierId, CancellationToken cancellationToken = default);
	Task UpdateProductList(UpdateProductDto productDto, CancellationToken cancellationToken = default);
	Task DeleteProduct(int productId, CancellationToken cancellationToken = default);

	Task<string?> CreateOrder(CreateOrderDto orderDto, CancellationToken cancellationToken = default);
	Task<ReadOrderDto?> GetOrderAsync(int orderId, CancellationToken cancellationToken = default);
	Task<List<ReadOrderDto>> GetAllOrdersBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default);
	Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken = default);


}
