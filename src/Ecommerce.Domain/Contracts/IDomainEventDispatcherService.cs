using Ecommerce.SharedKernel.Common;
using Ecommerce.SharedKernel.Contracts;

namespace Ecommerce.Domain.Contracts;

public interface IDomainEventDispatcherService
{
    Task PublishDomainEvent(IDomainEvent @event, CancellationToken cancellationToken = default);    
}
