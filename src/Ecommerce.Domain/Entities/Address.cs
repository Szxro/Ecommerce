using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class Address : Entity
{
    public Address()
    {
        UserAddresses = new HashSet<UserAddress>();
    }

    public int StreetNumber { get; set; }

    public string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public Country Country { get; set; } = null!;

    public int CountryId { get; set; }

    public ICollection<UserAddress> UserAddresses { get; set; }
}
