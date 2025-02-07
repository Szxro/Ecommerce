namespace Ecommerce.SharedKernel.Common;

public abstract class AuditableEntity
{
    public DateTimeOffset CreatedAtUtc { get; set; }

    public DateTimeOffset ModifiedAtUtc { get; set; }
}
