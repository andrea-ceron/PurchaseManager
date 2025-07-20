using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PurchaseManager.Repository.Model;
using PurchaseManager.Shared.DTO;
using System.Text.Json;
using Utility.Kafka.MessageHandlers;

namespace PurchaseManager.Business.Factory;

    public static class TransactionalOutboxFactory
    {

	public static TransactionalOutbox CreateInsert(RawMaterialDtoForKafka dto) => Create(dto, Operations.Insert);
	public static TransactionalOutbox CreateUpdate(RawMaterialDtoForKafka dto, RawMaterial previousState) => Create(dto, Operations.Update, previousState);
	public static TransactionalOutbox CreateDelete(RawMaterialDtoForKafka dto) => Create(dto, Operations.Delete);
	public static TransactionalOutbox CreateCompensationInsert(RawMaterialDtoForKafka dto) => Create(dto, Operations.CompensationInsert);
	public static TransactionalOutbox CreateCompensationDelete(RawMaterialDtoForKafka dto) => Create(dto, Operations.CompensationDelete);
	public static TransactionalOutbox CreateCompensationUpdate(RawMaterialDtoForKafka dto) => Create(dto, Operations.CompensationUpdate);


	private static TransactionalOutbox Create(RawMaterialDtoForKafka dto, string operation, RawMaterial? previousState = null) => Create(nameof(RawMaterialDtoForKafka), dto, operation, previousState);
	private static TransactionalOutbox Create<TDTO, TModel>(string table, TDTO dto, string operation, TModel? model) 
		where TDTO : class 
		where TModel : class, new()
	{

		OperationMessage<TDTO, TModel> opMsg = new OperationMessage<TDTO, TModel>()
		{
			Dto = dto,
			Operation = operation,
			previousState = model
		};

		return new TransactionalOutbox()
		{
			Table = table,
			Message = JsonSerializer.Serialize(opMsg)
		};
	}

	public static OperationMessage<TDTO, TModel> Deserialize<TDTO, TModel>(string json) where TDTO : class where TModel : class, new()
	{
		return JsonSerializer.Deserialize<OperationMessage<TDTO, TModel>>(json)!;
	}


}
