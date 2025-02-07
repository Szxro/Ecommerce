using FluentValidation;

namespace Ecommerce.Infrastructure.Options.Database.Validations;

public class DatabaseOptionsValidator : AbstractValidator<DatabaseOptions>
{
    public DatabaseOptionsValidator()
    {
        RuleFor(x => x.ConnectionString)
          .NotNull().WithMessage("The {PropertyName} can't be null")
          .NotEmpty().WithMessage("The {PropertyName} can't be empty");

        RuleFor(x => x.CommandTimeout)
            .NotNull().WithMessage("The {PropertyName} can't be null")
            .NotEmpty().WithMessage("The {PropertyName} can't be empty")
            .GreaterThan(0).WithMessage("The {PropertyName} must be greather than 0");

        RuleFor(x => x.EnableDetailedErrors)
            .NotNull().WithMessage("The {PropertyName} can't be null")
            .NotEmpty().WithMessage("The {PropertyName} can't be empty");

        RuleFor(x => x.EnableSensitiveDataLogging)
            .NotNull().WithMessage("The {PropertyName} can't be null")
            .NotEmpty().WithMessage("The {PropertyName} can't be empty");
    }
}
