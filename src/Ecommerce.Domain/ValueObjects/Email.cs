using Ecommerce.SharedKernel.Common.Primitives;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Ecommerce.Domain.ValueObjects;

public sealed class Email
{
    public const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public string Value { get; }

    [JsonConstructor]
    private Email(string value)
    {
        Value = value;
    }

    public static bool IsValid(string? email, out Result<Email>? validEmail)
    {
        validEmail = null;

        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        if (!Regex.IsMatch(email, EmailPattern, RegexOptions.CultureInvariant))
        {
            return false;
        }

        validEmail = Result<Email>.Success(new Email(email));
        return true;
    }
}
