using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class ShoppingCartDetails : IntermediaryEntity
{
    public ShoppingCart ShoppingCart { get; set; } = null!;

    public int ShoppingCartId { get; set; }

    public Product Product { get; set; } = null!;

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public bool IsRemoved { get; set; }
}