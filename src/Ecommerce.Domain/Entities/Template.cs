using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class Template : Entity
{
    public Template()
    {
        UserTemplates = new HashSet<UserTemplate>();
        TemplateVariables = new HashSet<TemplateVariable>();
    }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public bool IsDefault { get; set; }

    public int TemplateCategoryId { get; set; }

    public TemplateCategory TemplateCategory { get; set; } = null!;

    public ICollection<TemplateVariable> TemplateVariables { get; set; }

    public ICollection<UserTemplate> UserTemplates { get; set; } 
}
