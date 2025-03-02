namespace Ecommerce.SharedKernel.Common;

public abstract class AuditableEntity
{
    public DateTime CreatedAtUtc { get; set; }

    public DateTime ModifiedAtUtc { get; set; }
}
