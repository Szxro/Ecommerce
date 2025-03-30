using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class UserAddress : IntermediaryEntity
{
    public User User { get; set; } = null!;

    public int UserId { get; set; }

    public Address Address { get; set; } = null!;

    public int AddressId { get; set; }

    public bool IsDefault { get; set; }
}