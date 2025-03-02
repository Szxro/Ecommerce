namespace Ecommerce.SharedKernel.Contracts;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }

    public DateTime DeletedAtUtc { get; set; }
}
