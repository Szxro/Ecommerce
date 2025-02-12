using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public sealed class EmailCode : Entity
{
    public string Code { get; set; } = string.Empty;

    public bool IsExpired { get; set; }

    public bool IsRevoked { get; set; }

    public bool IsUsed { get; set; }

    public DateTime ExpirationDateAtUtc { get; set; } = DateTime.Now.AddMinutes(30);

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
