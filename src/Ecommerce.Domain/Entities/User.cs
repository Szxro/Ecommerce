using Ecommerce.Domain.ValueObjects;
using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public sealed class User : Entity
{
    public User()
    {
        Credentials = new HashSet<Credentials>();
        RefreshTokens = new HashSet<RefreshToken>();
        EmailCodes = new HashSet<EmailCode>();
        Templates = new HashSet<UserTemplate>();
        UserImages = new HashSet<UserImage>();
    }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public Username Username { get; set; } = null!;

    public Email Email { get; set; } = null!;

    public bool IsEmailVerified { get; set; }

    public bool IsAdmin { get; set; }

    public bool LockOutEnabled { get; set; } = true;

    public DateTime LockOutEndAtUtc { get; set; } = new DateTime(1999,01,01,00,00,00);

    public int AccessFailedCount { get; set; }

    public ICollection<Credentials> Credentials { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; }

    public ICollection<EmailCode> EmailCodes { get; set; }

    public ICollection<UserTemplate> Templates { get; set; }

    public ICollection<UserImage> UserImages { get; set; }
}
