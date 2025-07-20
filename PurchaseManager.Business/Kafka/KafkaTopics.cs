using Utility.Kafka.DependencyInjection;
namespace PurchaseManager.Business.Kafka;

public class KafkaTopicsOutput : AbstractOutputKafkaTopics
{
	public string RawMaterialPurchaseToStock { get; set; } = "rawMaterialPurchaseToStock";
	public override IEnumerable<string> GetTopics() => [RawMaterialPurchaseToStock];
}

public class KafkaTopicsInput : AbstractOutputKafkaTopics
{
	public string RawMaterialStockToPurchase { get; set; } = "rawMaterialStockToPurchase";
	public override IEnumerable<string> GetTopics() => [RawMaterialStockToPurchase];
}