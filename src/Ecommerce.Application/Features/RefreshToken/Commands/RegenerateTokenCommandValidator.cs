using FluentValidation;

namespace Ecommerce.Application.Features.RefreshTokens.Commands;

public class RegenerateTokenCommandValidator : AbstractValidator<RegenerateTokenCommand>
{
    public RegenerateTokenCommandValidator()
    {
        RuleFor(x => x.refreshToken)
            .NotNull().WithMessage("The refresh token can't be null")
            .NotEmpty().WithMessage("The refresh token can't be empty")
            .Must(IsBase64).When(x => !string.IsNullOrEmpty(x.refreshToken)).WithMessage("The provided refresh token is invalid");

        RuleFor(x => x.token)
            .NotNull().WithMessage("The token can't be null")
            .NotEmpty().WithMessage("The token can't be empty")
            .Matches(@"^[A-Za-z0-9-_]+\.([A-Za-z0-9-_]+\.?)[A-Za-z0-9-_]+$").WithMessage("The provided token is invalid");
    }

    // Getting the size of the base64 provided
    private int DecodedSize(string base64)
    {
        return (base64.Length * 3 + 3) / 4 - (base64.Length > 0 && base64[^1] == '=' ? base64.Length > 1 && base64[^2] == '=' ? 2 : 1 : 0);
    }

    private bool IsBase64(string base64)
    {
        int bufferSize = DecodedSize(base64);
        // Using Span to improve perfomance
        Span<byte> buffer = stackalloc byte[bufferSize]; // avoid array heap allocation
        return Convert.TryFromBase64String(base64, buffer, out _);
    }
}
