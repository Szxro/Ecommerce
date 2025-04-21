using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class ShoppingCart : Entity
{
    public ShoppingCart()
    {
        ShoppingCartDetails = new HashSet<ShoppingCartDetails>();
    }

    public User User { get; set; } = null!;

    public int UserId { get; set; }

    public bool IsSubmit { get; set; }

    public int TotalPrice { get; set; }

    public ICollection<ShoppingCartDetails> ShoppingCartDetails { get; set; }
}
