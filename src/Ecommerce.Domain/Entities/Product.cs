using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class Product : Entity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int ProductCategoryId { get; set; }

    public ProductCategory ProductCategory { get; set; } = null!;
}
