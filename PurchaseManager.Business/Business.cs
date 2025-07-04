﻿using AutoMapper;
using PurchaseManager.Business.Factory;
using Microsoft.Extensions.Logging;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Repository.Abstraction;
using PurchaseManager.Repository.Model;
using PurchaseManager.Shared.DTO;

namespace PurchaseManager.Business;

public class Business(IRepository repository, IMapper mapper, ILogger<Business> logger, IRawMaterialObserver observer) : IBusiness
{
	#region SupplierOrder
	public async Task<List<UpdateRawMaterialQuantity>> CreateSupplierOrderAsync(CreateSupplierOrderDto createSupplierOrderDto, CancellationToken ct = default)
	{
		SupplierOrder SupplierOrder = mapper.Map<SupplierOrder>(createSupplierOrderDto);
		List<RawMaterialSupplierOrder> RawMaterialList = mapper.Map<List<RawMaterialSupplierOrder>>(SupplierOrder.RawMaterialSupplierOrder);
		SupplierOrder.RawMaterialSupplierOrder = new List<RawMaterialSupplierOrder>();
		var result = await repository.CreateTransaction(async () =>
		{
			List<UpdateRawMaterialQuantity> payload = new();

			var newSupplierOrder = await repository.CreateSupplierOrderAsync(SupplierOrder, ct);
			await repository.SaveChangesAsync(ct);

			logger.LogInformation("Ordine creato con ID: {newSupplierOrderId}", newSupplierOrder.Id);

			foreach (var RawMaterial in RawMaterialList)
			{
				var readRawMaterial = await repository.GetRawMaterialByIdAsync	(RawMaterial.RawMaterialId, ct);
				if (readRawMaterial == null)
					throw new ExceptionHandler($"Prodotto con ID {RawMaterial.RawMaterialId} non trovato.", 404);
				if (readRawMaterial.SupplierId != newSupplierOrder.SupplierId)
					throw new ExceptionHandler("L'id del prodotto non corrisponde ad un prodotto venduto dal fornitore inserito.", 400);
				if (readRawMaterial.MinQuantityForSupplierOrder > RawMaterial.Quantity)
				{
					logger.LogInformation($"La quantità minima per l'ordine del fornitore è {readRawMaterial.MinQuantityForSupplierOrder}.");

					RawMaterial.Quantity = readRawMaterial.MinQuantityForSupplierOrder;
				}

				RawMaterial.SupplierOrderId = newSupplierOrder.Id;
				await repository.CreateRawMaterialSupplierOrderAsync(RawMaterial, ct);
				 
				readRawMaterial.AvailableQuantity += RawMaterial.Quantity;
				UpdateRawMaterialQuantity updateRawMaterialQuantity = new()
				{
					Id = readRawMaterial.Id,
					QuantityToAdd = RawMaterial.Quantity
				};
				payload.Add(updateRawMaterialQuantity);

				await repository.SaveChangesAsync(ct);
			}

			return payload;
		});
		return result;
	}

	public async Task<List<ReadSupplierOrderDto>?> GetAllSupplierOrdersBySupplierIdAsync(int SupplierId, CancellationToken ct = default)
	{
		List<SupplierOrder> ListOfSupplierOrder = await repository.GetSupplierOrderBySupplierIdAsync(SupplierId, ct);
		if (ListOfSupplierOrder == null) throw new ExceptionHandler("Nessun Ordine trovato", 404);
		List<ReadSupplierOrderDto> SupplierOrderList = mapper.Map<List<ReadSupplierOrderDto>>(ListOfSupplierOrder);
		return SupplierOrderList;
	}
	public async Task<ReadSupplierOrderDto> GetSupplierOrderByIdAsync(int SupplierOrderId, CancellationToken ct = default)
	{
		var SupplierOrder =  await repository.GetSupplierOrderByIdAsync(SupplierOrderId, ct);
		if (SupplierOrder == null ) throw new ExceptionHandler("Nessun Ordine trovato", 404);
		ReadSupplierOrderDto SupplierOrderdto = mapper.Map<ReadSupplierOrderDto>(SupplierOrder);
		return SupplierOrderdto;
	}
	#endregion

	#region Supplier
	public async Task CreateSupplierAsync(CreateSupplierDto supplierDto, CancellationToken ct = default)
	{
		Supplier supplier = mapper.Map<Supplier>(supplierDto);

		List<RawMaterial> RawMaterialList = mapper.Map<List<RawMaterial>>(supplierDto.RawMaterials);
		supplier.RawMaterials = new List<RawMaterial>();


		await repository.CreateTransaction(async() =>
		{

			var supplierRes = await repository.CreateSupplierAsync(supplier, ct);
			await repository.SaveChangesAsync(ct);
			logger.LogInformation("Supplier created with ID: {supplierRes}", supplierRes.Id);


			foreach (var item in RawMaterialList)
			{
				item.SupplierId = supplierRes.Id;
				await repository.CreateRawMaterialAsync(item, ct);
			}
			await repository.SaveChangesAsync(ct);

		});
	}
	public async Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default)
	{
		await repository.CreateTransaction(async () =>
		{
			List<SupplierOrder>? SupplierOrderList = await repository.GetSupplierOrderBySupplierIdAsync(supplierId, ct);

			foreach (var SupplierOrder in SupplierOrderList)
			{
				await repository.DeleteAllRawMaterialSupplierOrdersBySupplierOrderIdAsync(SupplierOrder.Id, ct);
			}
			await repository.SaveChangesAsync(ct);

			await repository.DeleteAllRawMaterialsBySupplierIdAsync(supplierId, ct);
			await repository.SaveChangesAsync(ct);

			await repository.DeleteAllSupplierOrdersBySupplierIdAsync(supplierId, ct);
			await repository.SaveChangesAsync(ct);

			await repository.DeleteSupplierAsync(supplierId, ct);
			await repository.SaveChangesAsync(ct);

		});
	}
	public async Task<ReadSupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default)
	{
		
		Supplier? supplier =  await repository.GetSupplierByIdAsync(supplierId, ct);
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
		await repository.SaveChangesAsync(ct);
	}
	#endregion

	#region RawMaterial
	public async Task CreateListOfRawMaterialsAsync(IEnumerable<CreateRawMaterialDto> RawMaterialDto, CancellationToken ct = default)
	{
		List<RawMaterial> RawMaterialList = mapper.Map<List<RawMaterial>>(RawMaterialDto);
		await repository.CreateTransaction(async () =>
		{
			foreach (var RawMaterial in RawMaterialList)
			{
				var res = await repository.CreateRawMaterialAsync(RawMaterial, ct);
				await repository.SaveChangesAsync(ct);
				var recordKafka = mapper.Map<RawMaterialDtoForKafka>(res);
				var record = TransactionalOutboxFactory.CreateInsert(recordKafka);
				await repository.InsertTransactionalOutboxAsync(record, ct);
			}
				await repository.SaveChangesAsync(ct);
		});
		observer.AddRawMaterial.OnNext(1);

	}


	public async Task UpdateRawMaterialAsync(UpdateRawMaterialDto RawMaterialDto, CancellationToken ct = default)
	{
		await repository.CreateTransaction(async () =>
		{
			var model = mapper.Map<RawMaterial>(RawMaterialDto);
			var updateRawMaterial = await repository.UpdateRawMaterialAsync(model, ct);
			await repository.SaveChangesAsync(ct);
			var dtoUpdateRawMaterial = mapper.Map<RawMaterialDtoForKafka>(RawMaterialDto);
			var record = TransactionalOutboxFactory.CreateUpdate(dtoUpdateRawMaterial);
			await repository.InsertTransactionalOutboxAsync(record, ct);
			await repository.SaveChangesAsync(ct);

		});
		observer.AddRawMaterial.OnNext(1);

	}
	public async Task<List<ReadRawMaterialDto>> GetRawMaterialListBySupplierId(int SupplierId, CancellationToken ct = default)
	{
		var RawMaterialList = await repository.GetAllRawMaterialBySupplierIdAsync(SupplierId, ct);
		if (RawMaterialList == null || RawMaterialList.Count == 0)
		{
			throw new ExceptionHandler("Nessun prodotto trovato per questo fornitore", 404);
		}
		return mapper.Map<List<ReadRawMaterialDto>>(RawMaterialList);
	}
	public async Task DeleteRawMaterialAsync(int RawMaterialId, CancellationToken ct = default)
	{
		var listOfRawMaterialSupplierOrder = await repository.GetAllRawMaterialSupplierOrderByRawMaterialIdAsync(RawMaterialId, ct);
		var RawMaterialDto = await repository.GetRawMaterialByIdAsync(RawMaterialId, ct);
		await repository.CreateTransaction(async () =>
		{
			if (listOfRawMaterialSupplierOrder.Count > 0) throw new ExceptionHandler("Non e possibile eliminare il prodotto, eliminare gli ordini corrispondenti", 403);
			await repository.DeleteRawMaterialAsync(RawMaterialId, ct);
			await repository.SaveChangesAsync(ct);
			var dtoUpdateRawMaterial = mapper.Map<RawMaterialDtoForKafka>(RawMaterialDto);
			var record = TransactionalOutboxFactory.CreateUpdate(dtoUpdateRawMaterial);
			await repository.InsertTransactionalOutboxAsync(record, ct);
			await repository.SaveChangesAsync(ct);
		});
		observer.AddRawMaterial.OnNext(1);

	}
	#endregion







}
