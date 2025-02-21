using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class TemplateCategory : Entity
{
    public TemplateCategory()
    {
        Templates = new HashSet<Template>();
    }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<Template> Templates { get; set; }
}
