using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class ShoppingCartProduct : IntermediaryEntity
{
    public ShoppingCart ShoppingCart { get; set; } = null!;

    public int ShoppingCartId { get; set; }

    public Product Product { get; set; } = null!;

    public int ProductId { get; set; }

    public int Quantity { get; set; }
}