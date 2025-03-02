using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class Variable : Entity
{
    public Variable()
    {
        TemplateVariables = new HashSet<TemplateVariable>();
    }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int VariableTypeId { get; set; }

    public VariableType VariableType { get; set; } = null!;

    public ICollection<TemplateVariable> TemplateVariables { get; set; }
}
