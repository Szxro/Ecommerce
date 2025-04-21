using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class Product : Entity
{
    public Product()
    {
        ProductImages = new HashSet<ProductImage>();
        ShoppingCartDetails = new HashSet<ShoppingCartDetails>();
    }
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Price { get; set; }

    public int ProductCategoryId { get; set; }

    public ProductCategory ProductCategory { get; set; } = null!;

    public ICollection<ProductImage> ProductImages { get; set; }

    public ICollection<ShoppingCartDetails> ShoppingCartDetails { get; set; }
}
