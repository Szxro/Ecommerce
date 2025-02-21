using MediatR;

namespace Ecommerce.SharedKernel.Contracts;

public interface IDomainEvent : INotification
{
    public string Category { get; }
};
