using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public sealed class RefreshToken : Entity
{
    public string Token { get; set; } = string.Empty;

    public bool IsExpired { get; set; }

    public bool IsRevoked { get; set; }

    public bool IsUsed { get; set; }

    public DateTimeOffset ExpirationDateAtUtc { get; set; } = DateTimeOffset.Now.AddMinutes(30);

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}