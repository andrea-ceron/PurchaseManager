using PurchaseManager.Repository.Model;
using PurchaseManager.Shared.DTO;
using System.Text.Json;
using Utility.Kafka.MessageHandlers;

namespace PurchaseManager.Business.Factory;

    public static class TransactionalOutboxFactory
    {

	public static TransactionalOutbox CreateInsert(RawMaterialDtoForKafka dto) => Create(dto, Operations.Insert);
	public static TransactionalOutbox CreateUpdate(RawMaterialDtoForKafka dto) => Create(dto, Operations.Update);
	public static TransactionalOutbox CreateDelete(RawMaterialDtoForKafka dto) => Create(dto, Operations.Delete);


	private static TransactionalOutbox Create(RawMaterialDtoForKafka dto, string operation) => Create(nameof(RawMaterialDtoForKafka), dto, operation);
	private static TransactionalOutbox Create<TDTO>(string table, TDTO dto, string operation) where TDTO : class, new()
	{

		OperationMessage<TDTO> opMsg = new OperationMessage<TDTO>()
		{
			Dto = dto,
			Operation = operation
		};

		return new TransactionalOutbox()
		{
			Table = table,
			Message = JsonSerializer.Serialize(opMsg)
		};
	}


}
