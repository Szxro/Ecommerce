using FluentValidation;

namespace Ecommerce.Application.Features.Users.Commands.LoginUserCommand;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command.username)
            .NotNull().WithMessage("The username can't be null")
            .NotEmpty().WithMessage("The username can't be empty")
            .MinimumLength(3).WithMessage("The username must contain at least three characters.")
            .MaximumLength(20).WithMessage("The username must not exceed 20 characters.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("The username can only contain letters, numbers, and underscores.");

        RuleFor(command => command.password)
            .NotNull().WithMessage("The password can't be null")
            .NotEmpty().WithMessage("The password can't be empty");
    }
}
