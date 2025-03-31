using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Ecommerce.Infrastructure.Services;

[Inject(ServiceLifetime.Transient)]
class DomainEventDispatcherService : IDomainEventDispatcherService
{
    private readonly ILogger<DomainEventDispatcherService> _logger;
    private readonly IPublisher _publisher;
    private readonly IExponentialBackoffService _exponentialBackoffService;

    public DomainEventDispatcherService(
        ILogger<DomainEventDispatcherService> logger,
        IPublisher publisher,
        IExponentialBackoffService exponentialBackoffService)
    {
        _logger = logger;
        _publisher = publisher;
        _exponentialBackoffService = exponentialBackoffService;
    }
    public async Task PublishDomainEvent(IDomainEvent @event, CancellationToken cancellationToken = default)
    {       
        try
        {
            await _publisher.Publish(@event, cancellationToken);

            _logger.LogInformation("Succesfully publish the event {eventName}", @event.GetType().Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An unexpected error happen while trying to publish the domain event {eventName} with the error message : {message}",
                @event.GetType().Name,
                ex.Message);

            await _exponentialBackoffService.RetryWithBackoff(
                () => _publisher.Publish(@event, cancellationToken),
                cancellationToken: cancellationToken);
        }
    }
}
