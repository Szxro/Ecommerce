using FluentValidation;

namespace Ecommerce.Application.Features.Users.Commands.ResetPasswordCommand;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.resetCode)
            .NotEmpty().WithMessage("The reset code cant be empty.")
            .NotNull().WithMessage("The reset code cant be null");

        RuleFor(x => x.newPassword)
            .NotEmpty().WithMessage("The reset code cant be empty.")
            .NotNull().WithMessage("The reset code cant be null")
            .MinimumLength(8).WithMessage("The password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("The password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("The password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("The password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("The password must contain at least one special character.");
    }
}
