using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class UserTemplate : IntermediaryEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int TemplateId { get; set; }

    public Template Template { get; set; } = null!;
}
