using MediatR;

namespace Ecommerce.SharedKernel.Contracts;

public interface IDomainEventHandler<TNotification> : INotificationHandler<TNotification>
    where TNotification : IDomainEvent
{ }
