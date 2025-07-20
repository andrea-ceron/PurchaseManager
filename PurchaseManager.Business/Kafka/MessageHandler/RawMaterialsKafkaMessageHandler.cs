using AutoMapper;
using Microsoft.Extensions.Logging;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Business.Factory;
using PurchaseManager.Repository.Abstraction;
using PurchaseManager.Repository.Model;
using PurchaseManager.Shared.DTO;
using Utility.Kafka.ExceptionManager;
namespace PurchaseManager.Business.Kafka.MessageHandler;

public class RawMaterialsKafkaMessageHandler
	(ILogger<RawMaterialsKafkaMessageHandler> logger,
	IRepository repository,
	IMapper map,
	ErrorManagerMiddleware errorManager,
	IRawMaterialObserver observer)
	: AbstractMessageHandler<RawMaterialDtoForKafka, RawMaterial>(errorManager, map)
{
	protected override Task DeleteDto(RawMaterial? messageDto, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}

	protected async override Task InsertDto(RawMaterial? domainDto, CancellationToken ct = default)
	{
		if (domainDto == null)
		{
			logger.LogWarning("Ricevuto null RawMaterialDtoForKafka, Operazione di inserimento saltata.");
			return;
		}

		logger.LogInformation("Ricevuto RawMaterialDtoForKafka, Operazione di inserimento in corso.");
		logger.LogInformation("Id domanDto: {Id}, SupplierId: {SupplierId}, MinQuantity: {MinQuantity}, SupplierCode: {SupplierCode}",
			domainDto.Id, domainDto.SupplierId, domainDto.MinQuantityForSupplierOrder, domainDto.SupplierRawMaterialCode);

		var result = await repository.CreateRawMaterialAsync(domainDto, ct);
		await repository.SaveChangesAsync(ct);
		var recordKafka = map.Map<RawMaterialDtoForKafka>(result);
		var record = TransactionalOutboxFactory.CreateCompensationInsert(recordKafka);
		await repository.InsertTransactionalOutboxAsync(record, ct);
		await repository.SaveChangesAsync(ct);
		observer.AddRawMaterialPurchaseToStock.OnNext(1);
	}

	protected override Task UpdateDto(RawMaterial? messageDto, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}
}
