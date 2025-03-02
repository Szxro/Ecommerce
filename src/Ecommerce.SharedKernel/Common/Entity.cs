using Ecommerce.SharedKernel.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.SharedKernel.Common;

public abstract class Entity 
    : AuditableEntity, IEntity, ISoftDeletable
{
    public int Id { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DeletedAtUtc { get; set; } = DateTime.MinValue;

    private List<IDomainEvent> domainEvents = new List<IDomainEvent>();

    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvent => domainEvents;

    public void AddEvent(IDomainEvent @event)
    {
        domainEvents.Add(@event);
    }

    public void RemoveEvent(IDomainEvent @event)
    {
        domainEvents.Remove(@event);
    }

    public void ClearEvents()
    {
        domainEvents.Clear();
    }
}
