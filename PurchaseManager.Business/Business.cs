using AutoMapper;
using PurchaseManager.Business.Factory;
using Microsoft.Extensions.Logging;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Repository.Abstraction;
using PurchaseManager.Repository.Model;
using PurchaseManager.Shared.DTO;

namespace PurchaseManager.Business;

public class Business(IRepository repository, IMapper mapper, ILogger<Business> logger, IRawMaterialObserver observer) : IBusiness
{
	#region Order
	public async Task CreateOrderAsync(CreateOrderDto createOrderDto, CancellationToken ct = default)
	{
		Order order = mapper.Map<Order>(createOrderDto);
		List<ProductOrder> productList = mapper.Map<List<ProductOrder>>(order.ProductOrder);
		order.ProductOrder = new List<ProductOrder>();

		await repository.CreateTransaction(async () =>{

		var newOrder = await repository.CreateOrderAsync(order, ct);
		await repository.SaveChanges(ct);

			logger.LogInformation("Ordine creato con ID: {newOrderId}", newOrder.Id);

			foreach (var product in productList)
			{
				var readProduct = await repository.GetProductById(product.ProductId, ct);
				if (readProduct == null)
					throw new ExceptionHandler($"Prodotto con ID {product.ProductId} non trovato.", 404);
				if (readProduct.SupplierId != newOrder.SupplierId)
					throw new ExceptionHandler("L'id del prodotto non corrisponde ad un prodotto venduto dal fornitore inserito.", 400);
				if(readProduct.MinQuantityForOrder > product.Quantity)
					throw new ExceptionHandler("Quantità richiesta inferriore alla quantità minima ordinabile.", 400);
				product.OrderId = newOrder.Id;
				await repository.CreateProductOrderAsync(product, ct);
				await repository.SaveChanges(ct);
			}
		});
	}
	public async Task DeleteOrderAsync(int OrderId, CancellationToken ct = default)
	{
		await repository.CreateTransaction(async () =>
		{
			await repository.DeleteAllProductOrdersByOrderIdAsync(OrderId, ct);
			await repository.SaveChanges(ct);
			await repository.DeleteOrderAsync(OrderId, ct);
			await repository.SaveChanges(ct);
		});
	}
	public async Task<List<ReadOrderDto>?> GetAllOrdersBySupplierIdAsync(int SupplierId, CancellationToken ct = default)
	{
		List<Order> ListOfOrder = await repository.GetOrderBySupplierIdAsync(SupplierId, ct);
		if (ListOfOrder == null) throw new ExceptionHandler("Nessun Ordine trovato", 404);
		List<ReadOrderDto> orderList = mapper.Map<List<ReadOrderDto>>(ListOfOrder);
		return orderList;
	}
	public async Task<ReadOrderDto> GetOrderByIdAsync(int OrderId, CancellationToken ct = default)
	{
		var Order =  await repository.GetOrderByIdAsync(OrderId, ct);
		if (Order == null ) throw new ExceptionHandler("Nessun Ordine trovato", 404);
		ReadOrderDto orderdto = mapper.Map<ReadOrderDto>(Order);
		return orderdto;
	}
	#endregion

	#region Supplier
	public async Task CreateSupplierAsync(CreateSupplierDto supplierDto, CancellationToken ct = default)
	{
		Supplier supplier = mapper.Map<Supplier>(supplierDto);

		List<Product> productList = mapper.Map<List<Product>>(supplierDto.Products);
		supplier.Products = new List<Product>();


		await repository.CreateTransaction(async() =>
		{

			var supplierRes = await repository.CreateSupplierAsync(supplier, ct);
			await repository.SaveChanges(ct);
			logger.LogInformation("Supplier created with ID: {supplierRes}", supplierRes.Id);


			foreach (var item in productList)
			{
				item.SupplierId = supplierRes.Id;
				await repository.CreateProductAsync(item, ct);
			}
			await repository.SaveChanges(ct);

		});
	}
	public async Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default)
	{
		await repository.CreateTransaction(async () =>
		{
			List<Order>? orderList = await repository.GetOrderBySupplierIdAsync(supplierId, ct);

			foreach (var order in orderList)
			{
				await repository.DeleteAllProductOrdersByOrderIdAsync(order.Id, ct);
			}
			await repository.SaveChanges(ct);

			await repository.DeleteAllProductsBySupplierIdAsync(supplierId, ct);
			await repository.SaveChanges(ct);

			await repository.DeleteAllOrdersBySupplierIdAsync(supplierId, ct);
			await repository.SaveChanges(ct);

			await repository.DeleteSupplier(supplierId, ct);
			await repository.SaveChanges(ct);

		});
	}
	public async Task<ReadSupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default)
	{
		
		Supplier? supplier =  await repository.GetSupplierById(supplierId, ct);
		if(supplier == null)
		{
			throw new ExceptionHandler("fornitore non trovato", 404);
		}
		ReadSupplierDto supplierDto = mapper.Map<ReadSupplierDto>(supplier);
		return supplierDto;
	}
	public async Task UpdateSupplierAsync(UpdateSupplierDto supplierDto, CancellationToken ct = default)
	{
		var model = mapper.Map<Supplier>(supplierDto);
		var updatedSupplier = await repository.UpdateSupplierAsync(model, ct);
		if(updatedSupplier == null)
			throw new ExceptionHandler("Supplier not updated", 400);
		await repository.SaveChanges(ct);
	}
	#endregion

	#region Product
	public async Task CreateListOfProductsAsync(IEnumerable<CreateProductDto> productDto, CancellationToken ct = default)
	{
		List<Product> productList = mapper.Map<List<Product>>(productDto);
		await repository.CreateTransaction(async () =>
		{
			foreach (var product in productList)
			{
				var res = await repository.CreateProductAsync(product, ct);
				await repository.SaveChanges(ct);
				var recordKafka = mapper.Map<ProductDtoForKafka>(res);
				var record = TransactionalOutboxFactory.CreateInsert(recordKafka);
				await repository.InsertTransactionalOutboxAsync(record, ct);
			}
				await repository.SaveChanges(ct);
		});
		observer.AddRawMaterial.OnNext(1);

	}


	public async Task UpdateProductAsync(UpdateProductDto productDto, CancellationToken ct = default)
	{
		await repository.CreateTransaction(async () =>
		{
			var model = mapper.Map<Product>(productDto);
			var updateProduct = await repository.UpdateProductAsync(model, ct);
			await repository.SaveChanges(ct);
			var dtoUpdateProduct = mapper.Map<ProductDtoForKafka>(productDto);
			var record = TransactionalOutboxFactory.CreateUpdate(dtoUpdateProduct);
			await repository.InsertTransactionalOutboxAsync(record, ct);
			await repository.SaveChanges(ct);

		});
		observer.AddRawMaterial.OnNext(1);

	}
	public async Task<List<ReadProductDto>> GetProductListBySupplierId(int SupplierId, CancellationToken ct = default)
	{
		var productList = await repository.GetAllProductBySupplierId(SupplierId, ct);
		if (productList == null || productList.Count == 0)
		{
			throw new ExceptionHandler("Nessun prodotto trovato per questo fornitore", 404);
		}
		return mapper.Map<List<ReadProductDto>>(productList);
	}
	public async Task DeleteProductAsync(int productId, CancellationToken ct = default)
	{
		var listOfProductOrder = await repository.GetAllProductOrderByProductIdAsync(productId, ct);
		var productDto = await repository.GetProductById(productId, ct);
		await repository.CreateTransaction(async () =>
		{
			if (listOfProductOrder.Count > 0) throw new ExceptionHandler("Non e possibile eliminare il prodotto, eliminare gli ordini corrispondenti", 403);
			await repository.DeleteProduct(productId, ct);
			await repository.SaveChanges(ct);
			var dtoUpdateProduct = mapper.Map<ProductDtoForKafka>(productDto);
			var record = TransactionalOutboxFactory.CreateUpdate(dtoUpdateProduct);
			await repository.InsertTransactionalOutboxAsync(record, ct);
			await repository.SaveChanges(ct);
		});
		observer.AddRawMaterial.OnNext(1);

	}
	#endregion







}
