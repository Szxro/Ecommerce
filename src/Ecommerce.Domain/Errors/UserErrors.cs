using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class UserErrors
{
    public static Error EmailNotUniqueOrInvalid
            => Error.Validation("Email is not valid or is already registered.");

    public static Error UsernameNotUniqueOrInvalid
            => Error.Validation("Username is not valid or is already registered.");

    public static Error UserNotFoundByUsername
        => Error.Validation("The user by the username provided was not found.");

    public static Error UserInvalidCredentials(int attempts)
        => Error.Validation($"The provided credentials are incorrect,retry count {attempts}/3");


    public static Error UserLockOut(string username, DateTime dateTime)
    {
        TimeSpan timeRemaining = dateTime - DateTime.UtcNow;

        return Error.Validation($"The user with the username '{username}' is lock out, lock out end in {FormatTimeRemaining(timeRemaining)}");
    }

    private static string FormatTimeRemaining(TimeSpan timeSpan)
    {
        return timeSpan switch
        {
            { TotalDays: > 1 } => $"{Math.Floor(timeSpan.TotalDays)} days",
            { TotalHours: > 1 } => $"{Math.Floor(timeSpan.TotalHours)} hours",
            { TotalMinutes: > 1 } => $"{Math.Floor(timeSpan.TotalMinutes)} minutes",
            _ => $"{(int)timeSpan.TotalSeconds} seconds"
        };
    }
}
