using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class TemplateVariable : IntermediaryEntity
{
    public int TemplateId { get; set; }

    public Template Template { get; set; } = null!;

    public int VariableId { get; set; }

    public Variable Variable { get; set; } = null!;
}