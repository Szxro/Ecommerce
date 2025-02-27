using FluentValidation;

namespace Ecommerce.Application.Features.Users.Commands.RegisterUserCommand;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.email)
          .NotNull().WithMessage("The email address can't be null.")
          .NotEmpty().WithMessage("The email address can't be empty.");

        RuleFor(command => command.password)
          .NotNull().WithMessage("The password can't be null.")
          .NotEmpty().WithMessage("The password can't be empty.")
          .MinimumLength(8).WithMessage("The password must be at least 8 characters long.")
          .Matches(@"[A-Z]").WithMessage("The password must contain at least one uppercase letter.")
          .Matches(@"[a-z]").WithMessage("The password must contain at least one lowercase letter.")
          .Matches(@"[0-9]").WithMessage("The password must contain at least one number.")
          .Matches(@"[\W_]").WithMessage("The password must contain at least one special character.");

        RuleFor(command => command.username)
          .NotNull().WithMessage("The username can't be null")
          .NotEmpty().WithMessage("The username can't be empty")
          .MinimumLength(3).WithMessage("The username must contain at least three characters.")
          .MaximumLength(20).WithMessage("The username must not exceed 20 characters.");

        RuleFor(command => command.firstName)
          .NotNull().WithMessage("The firstname can't be null.")
          .NotEmpty().WithMessage("The firstname can't be empty.")
          .MinimumLength(1).WithMessage("The firstname must contain at least one characters.");

        RuleFor(command => command.lastName)
          .NotNull().WithMessage("The lastname can't be null.")
          .NotEmpty().WithMessage("The lastname can't be empty.")
          .MinimumLength(1).WithMessage("The lastname must contain at least one characters.");
    }
}
