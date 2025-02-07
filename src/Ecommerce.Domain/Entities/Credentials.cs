using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public sealed class Credentials : Entity
{
    public string HashValue { get; set; } = string.Empty;

    public string SaltValue { get; set; } = string.Empty;

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}
