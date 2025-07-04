using Microsoft.EntityFrameworkCore;
using PurchaseManager.Repository.Abstraction;
using PurchaseManager.Repository.Model;
using System.Linq;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage;

namespace PurchaseManager.Repository;

public class Repository(PurchaseDbContext dbContext) : IRepository
{

	#region SupplierOrder
	public async Task<SupplierOrder?> CreateSupplierOrderAsync(SupplierOrder model, CancellationToken ct = default)
	{
		await dbContext.SupplierOrders.AddAsync(model, ct);
		return model;
	}
	public async Task DeleteSupplierOrderAsync(int SupplierOrderId, CancellationToken ct = default)
	{
		SupplierOrder? SupplierOrder = await GetSupplierOrderByIdAsync(SupplierOrderId, ct);
		if (SupplierOrder == null) return;
		dbContext.SupplierOrders.Remove(SupplierOrder);
	}
	public async Task<SupplierOrder?> GetSupplierOrderByIdAsync(int SupplierOrderId, CancellationToken ct = default)
	{
		return  await dbContext.SupplierOrders
			.Where(o => o.Id == SupplierOrderId)
			.Include(o => o.RawMaterialSupplierOrder)
			.ThenInclude(p => p.RawMaterial)
			.AsNoTracking()
			.SingleOrDefaultAsync(ct);
	}
	public  Task<List<SupplierOrder>> GetSupplierOrderBySupplierIdAsync(int supplierId, CancellationToken ct = default)
	{
		return  dbContext.SupplierOrders
			.Where(o => o.SupplierId == supplierId)
			.Include(o => o.RawMaterialSupplierOrder)
				.ThenInclude(po => po.RawMaterial)
			.AsNoTracking()
			.ToListAsync(ct);

	}
	public async Task DeleteAllSupplierOrdersBySupplierIdAsync(int supplierId, CancellationToken ct = default)
	{
		List<SupplierOrder> SupplierOrderList = await GetSupplierOrderBySupplierIdAsync(supplierId, ct);
		if (SupplierOrderList == null || SupplierOrderList.Count == 0) return;
		dbContext.SupplierOrders.RemoveRange(SupplierOrderList);
	}



	#endregion

	#region Supplier
	public async Task<Supplier> CreateSupplierAsync( Supplier model, CancellationToken ct = default)
	{
		await dbContext.AddAsync(model, ct);
		return model;
	}
	public async Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default)
	{
		var supplier = await GetSupplierByIdAsync(supplierId, ct);
		if (supplier == null) return;
		dbContext.Suppliers.Remove(supplier);
	}
	public async Task<Supplier?> GetSupplierByIdAsync(int supplierId, CancellationToken ct = default)
	{
		return await dbContext.Suppliers
			.Where(s => s.Id == supplierId)
			.Include(s => s.RawMaterials)
			.Include(s => s.SupplierOrders)
				.ThenInclude(o => o.RawMaterialSupplierOrder)  
			.AsNoTracking()  
			.SingleOrDefaultAsync(ct);

	}
	public async Task<Supplier?> UpdateSupplierAsync(Supplier model, CancellationToken ct = default)
	{
		Supplier? supplier = await GetSupplierByIdAsync(model.Id, ct);
		if (supplier == null) return null;
		dbContext.Suppliers.Update(model);
		return model;
	}
	#endregion

	#region RawMaterial
	public async Task<RawMaterial> CreateRawMaterialAsync(RawMaterial model, CancellationToken ct = default)
	{
		await dbContext.AddAsync(model, ct);
		return model;
	}
	public async Task DeleteRawMaterialAsync(int rawMaterialId, CancellationToken ct = default)
	{
		var model = await GetRawMaterialByIdAsync(rawMaterialId, ct);
		if(model == null) return;
		dbContext.RawMaterials.Remove(model);
	}
	public async Task<List<RawMaterial>> GetAllRawMaterialBySupplierIdAsync(int supplierId, CancellationToken ct = default)
	{
		return await  dbContext.RawMaterials.Where(o => o.SupplierId == supplierId).AsNoTracking().ToListAsync(ct);
	}
	public async Task<RawMaterial?> GetRawMaterialByIdAsync(int rawMaterialId, CancellationToken ct = default)
	{
		return await dbContext.RawMaterials.Where(p => p.Id == rawMaterialId).AsNoTracking().SingleOrDefaultAsync(ct);
	}
	public async Task<RawMaterial?> UpdateRawMaterialAsync(RawMaterial model, CancellationToken ct = default)
	{
		RawMaterial? RawMaterial = await GetRawMaterialByIdAsync(model.Id, ct);
		if (RawMaterial == null) return null;
		dbContext.RawMaterials.Update(model);
		return model;
	}
	public async Task DeleteAllRawMaterialsBySupplierIdAsync(int supplierId, CancellationToken ct = default)
	{
		List<RawMaterial>? RawMaterialList = await GetAllRawMaterialBySupplierIdAsync(supplierId, ct);
		if (RawMaterialList == null || RawMaterialList.Count == 0) return;
		dbContext.RawMaterials.RemoveRange(RawMaterialList);
	}
	#endregion

	#region RawMaterialSupplierOrder
	public async Task<RawMaterialSupplierOrder> CreateRawMaterialSupplierOrderAsync(RawMaterialSupplierOrder model, CancellationToken ct = default)
	{
		await dbContext.RawMaterialSupplierOrders.AddAsync(model, ct);
		return model;			
	}
	public async Task DeleteRawMaterialSupplierOrder(RawMaterialSupplierOrder rawMaterialSupplierOrder, CancellationToken ct = default)
	{
		dbContext.RawMaterialSupplierOrders.Remove(rawMaterialSupplierOrder);
	}

	public async Task<List<RawMaterialSupplierOrder>> GetAllRawMaterialSupplierOrderBySupplierOrderIdAsync(int SupplierOrderId, CancellationToken ct = default)
	{
		return await dbContext.RawMaterialSupplierOrders
			.Where(po => po.SupplierOrderId == SupplierOrderId)
			.AsNoTracking()
			.ToListAsync(ct);
	}

	public async Task<List<RawMaterialSupplierOrder>> GetAllRawMaterialSupplierOrderByRawMaterialIdAsync(int rawMaterialId, CancellationToken ct = default)
	{
		return await dbContext.RawMaterialSupplierOrders
			.Where(po => po.RawMaterialId == rawMaterialId)
			.AsNoTracking()
			.ToListAsync(ct);
	}



	public async Task DeleteAllRawMaterialSupplierOrdersBySupplierOrderIdAsync(int SupplierOrderId, CancellationToken ct = default)
	{
		var listOfRawMaterialSupplierOrder = await GetAllRawMaterialSupplierOrderBySupplierOrderIdAsync(SupplierOrderId, ct);
		if (listOfRawMaterialSupplierOrder == null || listOfRawMaterialSupplierOrder.Count == 0) return;
		dbContext.RawMaterialSupplierOrders.RemoveRange(listOfRawMaterialSupplierOrder);
	}
	#endregion

	#region TransactionalOtbox
	public async Task<IEnumerable<TransactionalOutbox>> GetAllTransactionalOutbox(CancellationToken ct = default)
	{
		return await  dbContext.TransactionalOutboxes
			.ToListAsync(ct);
	}
	public async Task DeleteTransactionalOutboxAsync(long id, CancellationToken cancellationToken = default)
	{
		dbContext.TransactionalOutboxes.Remove(await GetTransactionalOutboxByKeyAsync(id, cancellationToken) 
			?? throw new ArgumentException($"TransactionalOutbox con id {id} non trovato", nameof(id)));
	}

	public async Task<TransactionalOutbox?> GetTransactionalOutboxByKeyAsync(long id, CancellationToken cancellationToken = default)
	{
		return await dbContext.TransactionalOutboxes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
	}

	public async Task InsertTransactionalOutboxAsync(TransactionalOutbox transactionalOutbox, CancellationToken cancellationToken = default)
	{
		await dbContext.TransactionalOutboxes.AddAsync(transactionalOutbox);
	}


	#endregion

	public async Task SaveChangesAsync(CancellationToken ct = default)
	{
		await dbContext.SaveChangesAsync(ct);
	}
	public async Task CreateTransaction(Func<Task> action)
	{
		if (dbContext.Database.CurrentTransaction != null)
		{
			await action();
		}
		else
		{
			using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				await action();
				await transaction.CommitAsync();
			}
			catch
			{
				await transaction.RollbackAsync();
				throw;
			}
		}
	}

	public async Task<T> CreateTransaction<T>(Func<Task<T>> action)
	{
		if (dbContext.Database.CurrentTransaction != null)
		{
			return await action();
		}
		else
		{
			using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				var result = await action();
				await transaction.CommitAsync();
				return result;
			}
			catch
			{
				await transaction.RollbackAsync();
				throw;
			}
		}
	}

}
