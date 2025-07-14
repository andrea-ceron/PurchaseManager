using AutoMapper;
using PurchaseManager.Business.Factory;
using Microsoft.Extensions.Logging;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Repository.Abstraction;
using PurchaseManager.Repository.Model;
using PurchaseManager.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Linq.Expressions;

namespace PurchaseManager.Business;

public class Business(IRepository repository, IMapper mapper, ILogger<Business> logger, IRawMaterialObserver observer) : IBusiness
{
	#region SupplierOrder
	public async Task<List<UpdateRawMaterialQuantity>> CreateSupplierOrderAsync(CreateSupplierOrderDto createSupplierOrderDto, CancellationToken ct = default)
	{
		logger.LogDebug("Entra in CreateSupplierOrder");

		SupplierOrder SupplierOrder = mapper.Map<SupplierOrder>(createSupplierOrderDto);
		logger.LogDebug("SupplierOrder ricevuto: {SupplierOrder}", SupplierOrder);
		List<RawMaterialSupplierOrder> RawMaterialSupplierOrderList = mapper.Map<List<RawMaterialSupplierOrder>>(SupplierOrder.RawMaterialSupplierOrder);
		SupplierOrder.RawMaterialSupplierOrder = new List<RawMaterialSupplierOrder>();
		return await repository.CreateTransaction(async () =>
		{
			List<UpdateRawMaterialQuantity> payload = new();

			var newSupplierOrder = await repository.CreateSupplierOrderAsync(SupplierOrder, ct);
			await repository.SaveChangesAsync(ct);

			logger.LogInformation("Ordine creato con ID: {newSupplierOrderId}", newSupplierOrder.Id);

			foreach (var RawMaterialSupplierOrder in RawMaterialSupplierOrderList)
			{
				var readRawMaterial = await repository.GetRawMaterialByIdAsync(RawMaterialSupplierOrder.RawMaterialId, ct);
				if (readRawMaterial == null)
					throw new ExceptionHandler($"Prodotto con ID {RawMaterialSupplierOrder.RawMaterialId} non trovato.", 404);
				if (readRawMaterial.SupplierId != newSupplierOrder.SupplierId)
					throw new ExceptionHandler("L'id del prodotto non corrisponde ad un prodotto venduto dal fornitore inserito.", 400);
				if (readRawMaterial.MinQuantityForSupplierOrder > RawMaterialSupplierOrder.Quantity)
				{
					logger.LogInformation($"La quantità minima per l'ordine del fornitore è {readRawMaterial.MinQuantityForSupplierOrder}.");
					RawMaterialSupplierOrder.Quantity = readRawMaterial.MinQuantityForSupplierOrder;
				}
				RawMaterialSupplierOrder.SupplierOrderId = newSupplierOrder.Id;
				RawMaterialSupplierOrder.SupplierOrder = newSupplierOrder;
				await repository.CreateRawMaterialSupplierOrderAsync(RawMaterialSupplierOrder, ct);
				newSupplierOrder.RawMaterialSupplierOrder.Add(RawMaterialSupplierOrder);
				payload.Add(new UpdateRawMaterialQuantity
				{
					Id = readRawMaterial.Id,
					QuantityToAdd = RawMaterialSupplierOrder.Quantity
				});
			}
			await repository.SaveChangesAsync(ct);
			return payload;
		});
	}
	public async Task<List<ReadSupplierOrderDto>> GetAllSupplierOrdersBySupplierIdAsync(int SupplierId, CancellationToken ct = default)
	{
		List<SupplierOrder>? ListOfSupplierOrder = await repository.GetSupplierOrdersBySupplierIdAsync(SupplierId, ct);
		List<ReadSupplierOrderDto> SupplierOrderList = mapper.Map<List<ReadSupplierOrderDto>>(ListOfSupplierOrder);
		return SupplierOrderList;
	}
	public async Task<ReadSupplierOrderDto> GetSupplierOrderByIdAsync(int SupplierOrderId, CancellationToken ct = default)
	{
		var SupplierOrder =  await repository.GetSupplierOrderByIdAsync(SupplierOrderId, ct);
		ReadSupplierOrderDto SupplierOrderdto = mapper.Map<ReadSupplierOrderDto>(SupplierOrder);
		return SupplierOrderdto;
	}
	#endregion

	#region Supplier
	public async Task<ReadSupplierDto> CreateSupplierAsync(CreateSupplierDto supplierDto, CancellationToken ct = default)
	{

		Supplier supplier = mapper.Map<Supplier>(supplierDto);
		var supplierRes = await repository.CreateSupplierAsync(supplier, ct);
		await repository.SaveChangesAsync(ct);
		ReadSupplierDto readSupplierDto = mapper.Map<ReadSupplierDto>(supplierRes);
		logger.LogInformation("Supplier creato con ID: {supplierRes}", supplierRes.Id);
		return readSupplierDto;
	}
	public async Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default)
	{
		List<SupplierOrder> SupplierOrderList = await repository.GetSupplierOrdersBySupplierIdAsync(supplierId, ct);
		await repository.CreateTransaction(async () =>
		{
			foreach (var SupplierOrder in SupplierOrderList)
			{
				await repository.DeleteAllRawMaterialSupplierOrdersBySupplierOrderIdAsync(SupplierOrder.Id, ct);
			}
			await repository.DeleteAllRawMaterialsBySupplierIdAsync(supplierId, ct);
			await repository.DeleteAllSupplierOrdersBySupplierIdAsync(supplierId, ct);
			await repository.DeleteSupplierAsync(supplierId, ct);
			await repository.SaveChangesAsync(ct);
		});
	}
	public async Task<ReadSupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default)
	{
		Supplier? supplier = await repository.GetSupplierByIdAsync(supplierId, ct);
		ReadSupplierDto supplierDto = mapper.Map<ReadSupplierDto>(supplier);
		return supplierDto;
	}
	public async Task<ReadSupplierDto> UpdateSupplierAsync(UpdateSupplierDto supplierDto, CancellationToken ct = default)
	{

		var model = mapper.Map<Supplier>(supplierDto);
		var updatedSupplier = await repository.UpdateSupplierAsync(model, ct);
		await repository.SaveChangesAsync(ct);
		ReadSupplierDto readSupplierDto = mapper.Map<ReadSupplierDto>(updatedSupplier);
		return readSupplierDto;
	}
	#endregion

	#region RawMaterial
	public async Task<List<ReadRawMaterialDto>> CreateListOfRawMaterialsAsync(IEnumerable<CreateRawMaterialDto> RawMaterialDto, CancellationToken ct = default)
	{
		List<RawMaterial> RawMaterialList = mapper.Map<List<RawMaterial>>(RawMaterialDto);
		List<ReadRawMaterialDto> ReadRawMaterialList = new();
		await repository.CreateTransaction(async () =>
		{
			foreach (var RawMaterial in RawMaterialList)
			{
				var res = await repository.CreateRawMaterialAsync(RawMaterial, ct);
				await repository.SaveChangesAsync(ct);
				var recordKafka = mapper.Map<RawMaterialDtoForKafka>(res);
				var record = TransactionalOutboxFactory.CreateInsert(recordKafka);
				await repository.InsertTransactionalOutboxAsync(record, ct);
				ReadRawMaterialList.Add(mapper.Map<ReadRawMaterialDto>(res));
			}
			await repository.SaveChangesAsync(ct);
		});
		observer.AddRawMaterial.OnNext(1);
		if(ReadRawMaterialList.Count == 0)
			throw new ExceptionHandler("Nessun prodotto inserito, controllare i dati inseriti", 400);
		return ReadRawMaterialList;

	}
	public async Task CreateRawMaterialsWithoutNotificationAsync(RawMaterial previousState, CancellationToken ct = default)
	{
		CreateRawMaterialDto backupRawMaterial = new()
		{
			SupplierId = previousState.SupplierId,
			MinQuantityForSupplierOrder = previousState.MinQuantityForSupplierOrder,
			RawMaterialName = previousState.RawMaterialName,
			Price = previousState.Price,
			SupplierRawMaterialCode = previousState.SupplierRawMaterialCode,
		};
		var model = mapper.Map<RawMaterial>(backupRawMaterial);
		var res = await repository.CreateRawMaterialAsync(model, ct);
		await repository.SaveChangesAsync(ct);
		var recordKafkaForInsert = mapper.Map<RawMaterialDtoForKafka>(res);
		var recordKafkaForDelete = mapper.Map<RawMaterialDtoForKafka>(previousState);
		var recordForDelete = TransactionalOutboxFactory.CreateCompensationDelete(recordKafkaForDelete);
		var recordForInsert = TransactionalOutboxFactory.CreateCompensationInsert(recordKafkaForInsert);
		await repository.InsertTransactionalOutboxAsync(recordForDelete, ct);
		await repository.InsertTransactionalOutboxAsync(recordForInsert, ct);
		await repository.SaveChangesAsync(ct);
	}
	public async Task UpdateRawMaterialAsync(UpdateRawMaterialDto RawMaterialDto, CancellationToken ct = default)
	{
		await repository.CreateTransaction(async () =>
		{
			var currentStateElement = await repository.GetRawMaterialByIdAsync((int)RawMaterialDto.Id, ct);
			if (currentStateElement == null)
				throw new ExceptionHandler("RawMaterial non trovata");

			var originalCopy = mapper.Map<RawMaterial>(currentStateElement);

			mapper.Map(RawMaterialDto, currentStateElement);
			await repository.SaveChangesAsync(ct);

			var dtoUpdateRawMaterial = mapper.Map<RawMaterialDtoForKafka>(RawMaterialDto);
			var record = TransactionalOutboxFactory.CreateUpdate(dtoUpdateRawMaterial, originalCopy);
			await repository.InsertTransactionalOutboxAsync(record, ct);
			await repository.SaveChangesAsync(ct);
		});

		observer.AddRawMaterial.OnNext(1);
	}
	public async Task<ReadRawMaterialDto> GetRawMaterialById(int rawMaterialId, CancellationToken ct = default)
	{
		var RawMaterial = await repository.GetRawMaterialByIdAsync(rawMaterialId, ct);
		return mapper.Map<ReadRawMaterialDto>(RawMaterial);
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
			var record = TransactionalOutboxFactory.CreateDelete(dtoUpdateRawMaterial, RawMaterialDto);
			await repository.InsertTransactionalOutboxAsync(record, ct);
			await repository.SaveChangesAsync(ct);
		});
		observer.AddRawMaterial.OnNext(1);

	}
	#endregion
}
