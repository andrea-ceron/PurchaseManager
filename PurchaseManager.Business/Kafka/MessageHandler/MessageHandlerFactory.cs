using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utility.Kafka.Abstraction.MessageHandlers;
using Utility.Kafka.Services;
namespace PurchaseManager.Business.Kafka.MessageHandler;

public class MessageHandlerFactory(ILogger<ConsumerService<KafkaTopicsInput>> logger, IOptions<KafkaTopicsInput> optionsTopics) : IMessageHandlerFactory<string, string>
{
	private readonly KafkaTopicsInput _optionsTopics = optionsTopics.Value;

	public IMessageHandler<string, string> Create(string topic, IServiceProvider serviceProvider)
	{

		if (topic == _optionsTopics.RawMaterialStockToPurchase)
			return ActivatorUtilities.CreateInstance<RawMaterialsKafkaMessageHandler>(serviceProvider);

		throw new ArgumentOutOfRangeException(nameof(topic), $"Il topic '{topic}' non è gestito");
	}
}