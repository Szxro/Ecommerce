using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class ShoppingCart : Entity
{
    public ShoppingCart()
    {
        ShoppingCartProducts = new HashSet<ShoppingCartProduct>();
    }

    public User User { get; set; } = null!;

    public int UserId { get; set; }

    public bool IsSubmit { get; set; }

    public int? TotalPrice { get; set; }

    public ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }
}
