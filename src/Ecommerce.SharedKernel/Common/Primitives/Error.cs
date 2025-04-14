using Ecommerce.SharedKernel.Enums;
using System.Text.Json.Serialization;

namespace Ecommerce.SharedKernel.Common.Primitives;

public class Error
{
    public string ErrorCode { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    [JsonConstructor]
    protected Error(
        string errorCode,
        string description,
        ErrorType type)
    {
        ErrorCode = errorCode;
        Description = description;
        Type = type;
    }

    // Note: need to override some operator to check not the reference in memory (when the error is deserialize it check if the reference are equal)
    public override bool Equals(object? obj)
    {
        if (obj is not Error other) return false;
        // In this case is the values to check if the error are equal to another
        return ErrorCode == other.ErrorCode && Description == other.Description && Type == other.Type;
    }

    public override int GetHashCode() => HashCode.Combine(ErrorCode, Description, Type); // Need to be implement

    public static bool operator ==(Error a, Error b) => a?.Equals(b) ?? b is null;

    public static bool operator !=(Error a, Error b) => !(a == b);

    public static Error None = new Error(string.Empty, string.Empty, ErrorType.None);

    public static Error Validation(string description) => new Error("Error.Validation", description, ErrorType.Validation);

    public static Error Conflict(string description) => new Error("Error.Conflict", description, ErrorType.Conflict);

    public static Error NotFound(string description) => new Error("Error.NotFound", description, ErrorType.NotFound);
}