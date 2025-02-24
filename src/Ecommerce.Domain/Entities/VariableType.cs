using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class VariableType : Entity
{
    public VariableType()
    {
        Variables = new HashSet<Variable>();
    }

    public string Name { get; set; } = string.Empty;

    public ICollection<Variable> Variables { get; set; }
}
