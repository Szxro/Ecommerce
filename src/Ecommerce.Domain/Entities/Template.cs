using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class Template : Entity
{
    public Template()
    {
        Templates = new HashSet<UserTemplate>();
    }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public bool IsDefault { get; set; }

    public int TemplateCategoryId { get; set; }

    public TemplateCategory TemplateCategory { get; set; } = null!;

    public ICollection<UserTemplate> Templates { get; set; } 
}
