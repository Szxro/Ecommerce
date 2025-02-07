using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public sealed class User : Entity
{
    public User()
    {
        Credentials = new HashSet<Credentials>();
        RefreshTokens = new HashSet<RefreshToken>();
        EmailCodes = new HashSet<EmailCode>();
    }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    // Value Objects?
    public string Username { get; set; } = string.Empty; // Unique!

    public string Email { get; set; } = string.Empty; // Unique! 

    public bool IsEmailVerified { get; set; }

    public bool IsAdmin { get; set; }

    public bool LockOutEnabled { get; set; } = true;

    public DateTimeOffset LockOutEndAtUtc { get; set; } = new DateTimeOffset(1999,01,01,00,00,00,TimeSpan.Zero);

    public int AccessFailedCount { get; set; }

    public ICollection<Credentials> Credentials { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; }

    public ICollection<EmailCode> EmailCodes { get; set; }  
}
