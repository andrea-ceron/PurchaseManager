using PurchaseManager.Repository.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Repository.Abstraction;
using Utility.Kafka.Abstraction.Clients;
using Utility.Kafka.ExceptionManager;
using PurchaseManager.Shared.DTO;
using Microsoft.Extensions.Logging;
using Utility.Kafka.MessageHandlers;
using PurchaseManager.Business.Factory;
namespace PurchaseManager.Business.Kafka;

public class ProducerServiceWithSubscription(
	IServiceProvider serviceProvider,
	ErrorManagerMiddleware errormanager,
	IOptions<KafkaTopicsOutput> optionTopics
	, IServiceScopeFactory serviceScopeFactory
	, IProducerClient<string, string> producerClient
	, IRawMaterialsObservable observable,
	ILogger<ProducerServiceWithSubscription> logger

	)
	: Utility.Kafka.Services.ProducerServiceWithSubscription(serviceProvider, errormanager)
{
	protected override IEnumerable<string> GetTopics()
	{
		return optionTopics.Value.GetTopics();
	}

	protected override IDisposable Subscribe(TaskCompletionSource<bool> tcs)
	{
		return observable.AddRawMaterialPurchaseToStock.Subscribe((change) => tcs.TrySetResult(true));
	}

	protected override async Task OperationsAsync(CancellationToken cancellationToken)
	{
		using IServiceScope scope = serviceScopeFactory.CreateScope();
		IRepository repository = scope.ServiceProvider.GetRequiredService<IRepository>();
		IBusiness business = scope.ServiceProvider.GetRequiredService<IBusiness>();

		IEnumerable<TransactionalOutbox> transactionalOutboxes = (await repository.GetAllTransactionalOutbox(cancellationToken)).OrderBy(x => x.Id);
		if (!transactionalOutboxes.Any())
		{
			return;
		}

		foreach (TransactionalOutbox elem in transactionalOutboxes)
		{
			string topic = elem.Table switch
			{
				nameof(RawMaterialDtoForKafka) => optionTopics.Value.RawMaterialPurchaseToStock,
				_ => throw new ArgumentOutOfRangeException($"La tabella {elem.Table} non è prevista come topic nel Producer")
			};
			try
			{
				logger.LogInformation($"Inizio produzione, {elem.Id.ToString()}, {elem.Message}");
				await producerClient.ProduceAsync(topic, elem.Id.ToString(), elem.Message, null, cancellationToken);
				logger.LogInformation($"Eseguita produzione, {elem.Id.ToString()}, {elem.Message}");
				await repository.DeleteTransactionalOutboxAsync(elem.Id, cancellationToken);
				logger.LogInformation($"Eseguita eliminazione, {elem.Id.ToString()}, {elem.Message}");

				await repository.SaveChangesAsync(cancellationToken);
				logger.LogInformation("Produzione del messaggio per il topic {topic} con id {id}", topic, elem.Id);
			}
			catch(InvalidOperationException ex)
			{
				logger.LogError(ex, "Circuit Breaker aperto troppe volte. Blocco la produzione.");
				var res = (await repository.GetAllTransactionalOutbox(cancellationToken));

				switch (elem.Table)
				{
					case nameof(RawMaterialDtoForKafka):
						await RawMaterialCompensationOperation(elem, repository, business, cancellationToken);
						break;
					default:
						logger.LogError("Tabella non supportata per compensazione: {table}", elem.Table);
						break;
				}
				await repository.SaveChangesAsync(cancellationToken);
				continue;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Errore durante la produzione del messaggio per il topic {topic} con id {id}", topic, elem.Id);
				throw;
			}


		}
	}

	private async Task RawMaterialCompensationOperation(TransactionalOutbox elem, IRepository repository, IBusiness business, CancellationToken cancellationToken = default)
	{
			var opMsg = TransactionalOutboxFactory.Deserialize<RawMaterialDtoForKafka, RawMaterial>(elem.Message);
			switch (opMsg.Operation)
			{
				case Operations.Insert:
					await repository.DeleteRawMaterialAsync(opMsg.Dto.Id, cancellationToken);
					await repository.DeleteTransactionalOutboxAsync(elem.Id, cancellationToken);
					logger.LogWarning("Incontrata Label Insert. Compensazione Insert: eliminato rawMaterial con ID {id}", opMsg.Dto.Id);
					break;

				case Operations.Update:
					await repository.UpdateRawMaterialAsync(opMsg.previousState!, cancellationToken);
					await repository.DeleteTransactionalOutboxAsync(elem.Id, cancellationToken);
					logger.LogWarning("Incontrata Label Update. Compensazione Update: eliminato rawMaterial con ID {id}", opMsg.Dto.Id);
					break;

				case Operations.Delete:
					logger.LogWarning("Incontrata Label Delete. Compensazione Delete: nessuna operazione eseguita");
					break;

				case Operations.CompensationInsert:
					logger.LogWarning("Incontrata Compensazione Insert: nessuna azione eseguita dal produttore");
					break;

				case Operations.CompensationUpdate:
					logger.LogWarning("Incontrata Compensazione Update: nessuna azione eseguita dal produttore");
					break;

				case Operations.CompensationDelete:
					logger.LogWarning("Incontrata Compensazione Delete: nessuna azione eseguita dal produttore");
					break;

				default:
					logger.LogError("Operazione sconosciuta: {op}", opMsg.Operation);
					break;
			}
		}


	}

