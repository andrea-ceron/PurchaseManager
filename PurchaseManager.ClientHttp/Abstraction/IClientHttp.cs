using PurchaseManager.Shared.DTO;
namespace PurchaseManager.ClientHttp.Abstraction;

public interface IPurchaseManagerClientHttp
{
	Task<ReadSupplierDto> CreateSupplier(CreateSupplierDto supplierDto, CancellationToken cancellationToken = default);
    Task<ReadSupplierDto> GetSupplier(int supplierId, CancellationToken cancellationToken = default);
    Task<ReadSupplierDto> UpdateSupplier(UpdateSupplierDto supplierDto, CancellationToken cancellationToken = default);
	Task<string?> DeleteSupplier(int supplierId, CancellationToken cancellationToken = default);

	Task<string?> CreateRawMaterial(IEnumerable<CreateRawMaterialDto> payload, CancellationToken cancellationToken = default);
	Task<List<ReadRawMaterialDto>?> GetRawMaterialListOfSupplier(int SupplierId, CancellationToken cancellationToken = default);
	Task<string?> UpdateRawMaterial(UpdateRawMaterialDto RawMaterialDto, CancellationToken cancellationToken = default);
	Task<string?> DeleteRawMaterial(int RawMaterialId, CancellationToken cancellationToken = default);

	Task<List<UpdateRawMaterialQuantity>?> CreateSupplierOrder(CreateSupplierOrderDto SupplierOrderDto, CancellationToken cancellationToken = default);
	Task<ReadSupplierOrderDto?> GetSupplierOrderAsync(int SupplierOrderId, CancellationToken cancellationToken = default);
	Task<List<ReadSupplierOrderDto>> GetAllSupplierOrdersBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default);


}
