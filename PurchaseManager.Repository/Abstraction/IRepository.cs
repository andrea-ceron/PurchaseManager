using PurchaseManager.Repository.Model;

namespace PurchaseManager.Repository.Abstraction;

public interface IRepository
{
	#region Supplier
	public Task<Supplier> GetSupplierByIdAsync(int supplierId, CancellationToken ct = default);
	public Task<Supplier> CreateSupplierAsync(Supplier model, CancellationToken ct = default);
	public Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default);
	public Task<Supplier> UpdateSupplierAsync(Supplier model, CancellationToken ct = default);
	#endregion


	#region SupplierOrder
	public Task<SupplierOrder> CreateSupplierOrderAsync(SupplierOrder model, CancellationToken ct = default);
	public Task<SupplierOrder> GetSupplierOrderByIdAsync(int SupplierOrderId, CancellationToken ct = default);
	public Task<List<SupplierOrder>> GetSupplierOrdersBySupplierIdAsync(int supplierId, CancellationToken ct = default);
	public Task DeleteSupplierOrderAsync(int SupplierOrderId, CancellationToken ct = default);
	public Task DeleteAllSupplierOrdersBySupplierIdAsync(int supplierId, CancellationToken ct = default);

	#endregion

	#region RawMaterial
	public Task DeleteRawMaterialAsync (int rawMaterialId, CancellationToken ct = default);
	public Task<RawMaterial> GetRawMaterialByIdAsync(int rawMaterialId, CancellationToken ct = default);
	public Task<RawMaterial> CreateRawMaterialAsync(RawMaterial model, CancellationToken ct = default);
	public Task<RawMaterial> UpdateRawMaterialAsync(RawMaterial model, CancellationToken ct = default);
	public Task<List<RawMaterial>> GetAllRawMaterialBySupplierIdAsync(int rawMaterialId, CancellationToken ct = default);
	public Task DeleteAllRawMaterialsBySupplierIdAsync(int supplierId, CancellationToken ct = default);

	#endregion


	#region RawMaterialSupplierOrder
	public Task DeleteRawMaterialSupplierOrder(RawMaterialSupplierOrder rawMaterialSupplierOrder, CancellationToken ct = default);
	public Task<RawMaterialSupplierOrder> CreateRawMaterialSupplierOrderAsync(RawMaterialSupplierOrder model, CancellationToken ct = default);
	public Task DeleteAllRawMaterialSupplierOrdersBySupplierOrderIdAsync(int SupplierOrderId, CancellationToken ct = default);
	public Task<List<RawMaterialSupplierOrder>> GetAllRawMaterialSupplierOrderBySupplierOrderIdAsync(int SupplierOrderId, CancellationToken ct = default);
	public Task<List<RawMaterialSupplierOrder>> GetAllRawMaterialSupplierOrderByRawMaterialIdAsync(int rawMaterialId, CancellationToken ct = default);
	#endregion


	#region TransactionalOutbox
	public Task<IEnumerable<TransactionalOutbox>> GetAllTransactionalOutbox(CancellationToken ct = default);
	Task DeleteTransactionalOutboxAsync(long id, CancellationToken cancellationToken = default);
	public Task<TransactionalOutbox> GetTransactionalOutboxByKeyAsync(long id, CancellationToken cancellationToken = default);
	public Task InsertTransactionalOutboxAsync(TransactionalOutbox transactionalOutbox, CancellationToken cancellationToken = default);

	#endregion


	public Task SaveChangesAsync(CancellationToken ct = default);
	public Task CreateTransaction(Func<Task> action);
	public Task<T> CreateTransaction<T>(Func<Task<T>> action);


}
