using PurchaseManager.Repository.Model;
using PurchaseManager.Shared.DTO;
namespace PurchaseManager.Business.Abstraction;

public interface IBusiness
{
    #region Supplier
    public Task<ReadSupplierDto> CreateSupplierAsync(CreateSupplierDto supplierDto, CancellationToken ct = default);
    public Task<ReadSupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default);
    public Task<ReadSupplierDto> UpdateSupplierAsync(UpdateSupplierDto supplierDto, CancellationToken ct = default);
	public Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default);
    #endregion

    #region SupplierOrder 
    public Task<List<UpdateRawMaterialQuantity>> CreateSupplierOrderAsync(CreateSupplierOrderDto createSupplierOrderDto, CancellationToken ct = default);
    public Task<ReadSupplierOrderDto> GetSupplierOrderByIdAsync(int SupplierOrderId, CancellationToken ct = default);
    public Task<List<ReadSupplierOrderDto>> GetAllSupplierOrdersBySupplierIdAsync(int SupplierId, CancellationToken ct = default);
    #endregion

    #region RawMaterial
    public Task<IEnumerable<ReadRawMaterialDto>> CreateListOfRawMaterialsAsync(IEnumerable<CreateRawMaterialDto> RawMaterialDto, CancellationToken ct = default);
    public Task<ReadRawMaterialDto> GetRawMaterialById(int rawMaterialId, CancellationToken ct = default);
	public Task DeleteRawMaterialAsync(int RawMaterialId, CancellationToken ct = default);
	#endregion
}
