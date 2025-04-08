using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class ProductCategory : Entity
{
    public ProductCategory()
    {
        Products = new HashSet<Product>();
    }

    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; }
}
