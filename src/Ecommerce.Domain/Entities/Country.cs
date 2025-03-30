using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class Country : Entity
{
    public Country()
    {
        Addresses = new HashSet<Address>();
    }

    public string Name { get; set; } = string.Empty;
    
    public ICollection<Address> Addresses { get; set; }
}