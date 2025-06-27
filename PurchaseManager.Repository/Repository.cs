using Microsoft.EntityFrameworkCore;
using PurchaseManager.Repository.Abstraction;
using PurchaseManager.Repository.Model;
using System.Linq;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage;
using PurchaseManager.Repository.Model;

namespace PurchaseManager.Repository;

public class Repository(SupplierDbContext dbContext) : IRepository
{

	#region Order
	public async Task<Order?> CreateOrderAsync(Order model, CancellationToken ct = default)
	{
		await dbContext.Orders.AddAsync(model, ct);
		return model;
	}
	public async Task DeleteOrderAsync(int orderId, CancellationToken ct = default)
	{
		Order? order = await GetOrderByIdAsync(orderId, ct);
		if (order == null) return;
		dbContext.Orders.Remove(order);
	}
	public async Task<Order?> GetOrderByIdAsync(int OrderId, CancellationToken ct = default)
	{
		return  await dbContext.Orders
			.Where(o => o.Id == OrderId)
			.Include(o => o.ProductOrder)
			.ThenInclude(p => p.Product)
			.AsNoTracking()
			.SingleOrDefaultAsync(ct);
	}
	public  Task<List<Order>> GetOrderBySupplierIdAsync(int supplierId, CancellationToken ct = default)
	{
		return  dbContext.Orders
			.Where(o => o.SupplierId == supplierId)
			.Include(o => o.ProductOrder)
				.ThenInclude(po => po.Product)
			.AsNoTracking()
			.ToListAsync(ct);

	}
	public async Task DeleteAllOrdersBySupplierIdAsync(int supplierId, CancellationToken ct = default)
	{
		List<Order> orderList = await GetOrderBySupplierIdAsync(supplierId, ct);
		if (orderList == null || orderList.Count == 0) return;
		dbContext.Orders.RemoveRange(orderList);
	}



	#endregion

	#region Supplier
	public async Task<Supplier> CreateSupplierAsync( Supplier model, CancellationToken ct = default)
	{
		await dbContext.AddAsync(model, ct);
		return model;
	}
	public async Task DeleteSupplier(int supplierId, CancellationToken ct = default)
	{
		var supplier = await GetSupplierById(supplierId, ct);
		if (supplier == null) return;
		dbContext.Suppliers.Remove(supplier);
	}
	public async Task<Supplier?> GetSupplierById(int customerId, CancellationToken ct = default)
	{
		return await dbContext.Suppliers
			.Where(s => s.Id == customerId)
			.Include(s => s.Products)
			.Include(s => s.Orders)
				.ThenInclude(o => o.ProductOrder)  
			.AsNoTracking()  
			.SingleOrDefaultAsync(ct);

	}
	public async Task<Supplier?> UpdateSupplierAsync(Supplier model, CancellationToken ct = default)
	{
		Supplier? supplier = await GetSupplierById(model.Id, ct);
		if (supplier == null) return null;
		dbContext.Suppliers.Update(model);
		return model;
	}
	#endregion

	#region Product
	public async Task<Product> CreateProductAsync(Product model, CancellationToken ct = default)
	{
		await dbContext.AddAsync(model, ct);
		return model;
	}
	public async Task DeleteProduct(int productId, CancellationToken ct = default)
	{
		var model = await GetProductById(productId, ct);
		if(model == null) return;
		dbContext.Products.Remove(model);
	}
	public Task<List<Product>> GetAllProductBySupplierId(int supplierId, CancellationToken ct = default)
	{
		return dbContext.Products.Where(o => o.SupplierId == supplierId).AsNoTracking().ToListAsync(ct);
	}
	public async Task<Product?> GetProductById(int productId, CancellationToken ct = default)
	{
		return await dbContext.Products.Where(p => p.Id == productId).AsNoTracking().SingleOrDefaultAsync(ct);
	}
	public async Task<Product?> UpdateProductAsync(Product model, CancellationToken ct = default)
	{
		Product? product = await GetProductById(model.Id, ct);
		if (product == null) return null;
		dbContext.Products.Update(model);
		return model;
	}
	public async Task DeleteAllProductsBySupplierIdAsync(int supplierId, CancellationToken ct = default)
	{
		List<Product>? ProductList = await GetAllProductBySupplierId(supplierId, ct);
		if (ProductList == null || ProductList.Count == 0) return;
		dbContext.Products.RemoveRange(ProductList);
	}
	#endregion

	#region ProductOrder
	public async Task<ProductOrder> CreateProductOrderAsync(ProductOrder model, CancellationToken ct = default)
	{
		await dbContext.ProductOrders.AddAsync(model, ct);
		return model;			
	}
	public async Task DeleteProductOrder(ProductOrder productOrder, CancellationToken ct = default)
	{
		dbContext.ProductOrders.Remove(productOrder);
	}

	public async Task<List<ProductOrder>> GetAllProductOrderByOrderIdAsync(int orderId, CancellationToken ct = default)
	{
		return await dbContext.ProductOrders
			.Where(po => po.OrderId == orderId)
			.AsNoTracking()
			.ToListAsync(ct);
	}

	public async Task<List<ProductOrder>> GetAllProductOrderByProductIdAsync(int productId, CancellationToken ct = default)
	{
		return await dbContext.ProductOrders
			.Where(po => po.ProductId == productId)
			.AsNoTracking()
			.ToListAsync(ct);
	}



	public async Task DeleteAllProductOrdersByOrderIdAsync(int orderId, CancellationToken ct = default)
	{
		var listOfProductOrder = await GetAllProductOrderByOrderIdAsync(orderId, ct);
		if (listOfProductOrder == null || listOfProductOrder.Count == 0) return;
		dbContext.ProductOrders.RemoveRange(listOfProductOrder);
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

	public async Task SaveChanges(CancellationToken ct = default)
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

}
