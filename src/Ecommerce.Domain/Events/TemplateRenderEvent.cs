using Ecommerce.Domain.ValueObjects;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.SharedKernel.Enums;

namespace Ecommerce.Domain.Events;

public class TemplateRenderEvent : IDomainEvent
{
    public TemplateRenderEvent(
        TemplateCategory category,
        object model,
        Email email)
    {
        Category = category;
        Model = model;
        Email = email;
    }

    public TemplateCategory Category { get; }

    public object Model { get; }

    public Email Email { get; set; }
}
