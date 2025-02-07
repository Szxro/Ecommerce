using Ecommerce.SharedKernel.Common.Primitives;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Ecommerce.Domain.ValueObjects;

public sealed class Username
{
    public const string UsernamePattern = @"^[a-zA-Z0-9_]+$";

    public string Value { get; }

    [JsonConstructor]
    private Username(string value)
    {
        Value = value;
    }

    public static bool IsValid(string? username, out Result<Username>? validUsername)
    {
        validUsername = null;

        if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
        {
            return false;
        }

        if (!Regex.IsMatch(username, UsernamePattern, RegexOptions.CultureInvariant))
        {
            return false;
        }

        validUsername = Result<Username>.Success(new Username(username));
        return true;
    }
}