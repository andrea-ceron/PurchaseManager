using PurchaseManager.Repository.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Repository.Abstraction;
using PurchaseManager.Repository.Model;
using Utility.Kafka.Abstraction.Clients;
using Utility.Kafka.ExceptionManager;

namespace PurchaseManager.Business.Kafka;

public class ProducerServiceWithSubscription(
	IServiceProvider serviceProvider,
	ErrorManagerMiddleware errormanager,
	IOptions<KafkaTopicsOutput> optionTopics
	, IServiceScopeFactory serviceScopeFactory
	, IProducerClient<string, string> producerClient
	, IRawMaterialsObservable observable

	)
	: Utility.Kafka.Services.ProducerServiceWithSubscription(serviceProvider, errormanager)
{
	protected override IEnumerable<string> GetTopics()
	{
		return optionTopics.Value.GetTopics();
	}

	protected override IDisposable Subscribe(TaskCompletionSource<bool> tcs)
	{
		return observable.AddRawMaterial.Subscribe((change) => tcs.TrySetResult(true));
	}

	protected override async Task OperationsAsync(CancellationToken cancellationToken)
	{
		using IServiceScope scope = serviceScopeFactory.CreateScope();
		IRepository repository = scope.ServiceProvider.GetRequiredService<IRepository>();
		IEnumerable<TransactionalOutbox> transactionalOutboxes = (await repository.GetAllTransactionalOutbox(cancellationToken)).OrderBy(x => x.Id);
		if (!transactionalOutboxes.Any())
		{
			//logger.LogInformation($"Non ci sono TransactionalOutbox da elaborare");
			return;
		}

		foreach (TransactionalOutbox elem in transactionalOutboxes)
		{
			string topic = elem.Table switch
			{
				nameof(Product) => optionTopics.Value.RawMaterial,
				_ => throw new ArgumentOutOfRangeException($"La tabella {elem.Table} non è prevista come topic nel Producer")
			};
			try
			{
				await producerClient.ProduceAsync(topic, elem.Id.ToString(), elem.Message, null, cancellationToken);
				await repository.DeleteTransactionalOutboxAsync(elem.Id, cancellationToken);
				await repository.SaveChanges(cancellationToken);
			}
			catch (Exception ex)
			{
				//logger.LogError(ex, "Errore durante la produzione del messaggio per il topic {topic} con id {id}", topic, elem.Id);
				continue;
			}

			//logger.LogInformation("Eliminazione {groupMsg}...", groupMsg);
			
			//logger.LogInformation("Record eliminato");
		}
	}




	}

