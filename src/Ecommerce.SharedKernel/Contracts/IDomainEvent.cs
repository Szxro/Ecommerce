using Ecommerce.SharedKernel.Enums;
using MediatR;

namespace Ecommerce.SharedKernel.Contracts;

public interface IDomainEvent : INotification
{
    public TemplateCategory Category { get; }
};
