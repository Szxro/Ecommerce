namespace Ecommerce.SharedKernel.Common;

public abstract class Entity : AuditableEntity
{
    public int Id { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DeletedAtUtc { get; set; }
}
