using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Channels;
using Ecommerce.Infrastructure.Common;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Workers;

public class DomainEventDispatcherWorker : BaseWorker<DomainEventDispatcherWorker>
{
    private readonly DomainEventChannel _eventChannel;
    private readonly IDomainEventDispatcherService _dispatcherService;

    public DomainEventDispatcherWorker(
        ILogger<BaseWorker<DomainEventDispatcherWorker>> logger,
        DomainEventChannel eventChannel,
        IDomainEventDispatcherService dispatcherService) : base(logger)
    {
        _eventChannel = eventChannel;
        _dispatcherService = dispatcherService;
    }

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        await foreach (IDomainEvent @event in _eventChannel.ReadAllAsync(cancellationToken))
        {
            try
            {
                await _dispatcherService.PublishDomainEvent(@event, cancellationToken);
            }
            catch
            {
                _logger.LogError("Failed to publish the domain event {eventName}, retrying....", @event.GetType().Name);

                await _dispatcherService.RetryPublishDomainEvent(@event, cancellationToken: cancellationToken);
            }
        }
    }
}
