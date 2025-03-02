using Ecommerce.SharedKernel.Contracts;

namespace Ecommerce.SharedKernel.Common;

public class IntermediaryEntity : AuditableEntity, IEntity
{
    public int Id { get; set; }
}
