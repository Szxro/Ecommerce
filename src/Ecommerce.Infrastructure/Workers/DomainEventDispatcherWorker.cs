using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Channels;
using Ecommerce.Infrastructure.Common;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.SharedKernel.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Workers;

public class DomainEventDispatcherWorker : BaseWorker<DomainEventDispatcherWorker>
{
    private readonly DomainEventChannel _eventChannel;
    private readonly IPublisher _publisher;
    private readonly IExponentialBackoffService _exponentialBackoff;

    public DomainEventDispatcherWorker(
        ILogger<BaseWorker<DomainEventDispatcherWorker>> logger,
        DomainEventChannel eventChannel,        
        IPublisher publisher,
        IExponentialBackoffService exponentialBackoff) : base(logger)
    {
        _eventChannel = eventChannel;
        _publisher = publisher;
        _exponentialBackoff = exponentialBackoff;
    }

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        await foreach (IDomainEvent @event in _eventChannel.ReadAllAsync(cancellationToken))
        {
            try
            {
                await _publisher.Publish(@event, cancellationToken);

                _logger.LogInformation("Succesfully publish the event {eventName}", @event.GetType().Name);

            } catch (Exception ex)
            {
                _logger.LogError(
                    "An unexpected error happen while trying to publish the domain event {eventName} with the error message : {message}, retrying...",
                    @event.GetType().Name,
                    ex.Message);

                await _exponentialBackoff.RetryWithBackoff(
                    () => _publisher.Publish(@event, cancellationToken),                                                           
                    new BackOffOptions(3, 100, 5000, 2),
                    cancellationToken);
            }
        }
    }
}
