using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Kafka.DependencyInjection;

namespace PurchaseManager.Business.Kafka;

public class KafkaTopicsOutput : AbstractOutputKafkaTopics
{
	public string RawMaterial { get; set; } = "raw-materials";
	public override IEnumerable<string> GetTopics() => [RawMaterial];
}
