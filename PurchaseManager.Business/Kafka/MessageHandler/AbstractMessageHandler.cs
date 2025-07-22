using AutoMapper;
using Utility.Kafka.ExceptionManager;
using Utility.Kafka.MessageHandlers;
namespace PurchaseManager.Business.Kafka.MessageHandler;

public abstract class AbstractMessageHandler<TMessageDTO, TDomainDto>
	(ErrorManagerMiddleware errorManager, IMapper map)
	: OperationMessageHandlerBase<TMessageDTO>(errorManager)
	where TMessageDTO : class
	where TDomainDto : class 
{
	protected override async Task InsertAsync(TMessageDTO messageDto, CancellationToken cancellationToken = default)
	{
		await InsertDto(map.Map<TDomainDto>(messageDto), cancellationToken);
	}

	protected override async Task UpdateAsync(TMessageDTO messageDto, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	protected override async Task DeleteAsync(TMessageDTO messageDto, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	protected override async Task CompensationInsertAsync(TMessageDTO messageDto, CancellationToken cancellationToken = default)
	{
		await InsertDto(map.Map<TDomainDto>(messageDto), cancellationToken);
	}

	protected override async Task CompensationUpdateAsync(TMessageDTO messageDto, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	protected override async Task CompensationDeleteAsync(TMessageDTO messageDto, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}


	protected abstract Task InsertDto(TDomainDto? domainDto, CancellationToken ct = default);
	protected abstract Task UpdateDto(TDomainDto? messageDto, CancellationToken ct = default);
	protected abstract Task DeleteDto(TDomainDto? messageDto, CancellationToken ct = default);
	protected abstract Task CompensationInsertDto(TDomainDto? domainDto, CancellationToken ct = default);
	protected abstract Task CompensationUpdateDto(TDomainDto? messageDto, CancellationToken ct = default);
	protected abstract Task CompensationDeleteDto(TDomainDto? messageDto, CancellationToken ct = default);
}

