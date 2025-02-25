using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Events;

namespace Ecommerce.Application.Features.EmailCodes.Commands.VerifyEmailCode;

public record VerifyEmailCodeCommand(string emailCode) : ICommand;

public class VerifyEmailCodeCommandHandler : ICommandHandler<VerifyEmailCodeCommand>
{
    private readonly IEmailCodeRepository _emailCodeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyEmailCodeCommandHandler(
        IEmailCodeRepository emailCodeRepository,
        IUnitOfWork unitOfWork)
    {
        _emailCodeRepository = emailCodeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(VerifyEmailCodeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.emailCode) || string.IsNullOrWhiteSpace(request.emailCode))
        {
            return Result.Failure(Error.Validation("The email code can't be empty or have white spaces"));
        }

        EmailCode? foundCode = await _emailCodeRepository.GetEmailCodeByEmailCode(request.emailCode, cancellationToken);

        if (foundCode is null)
        {
            return Result.Failure(EmailCodeErrors.EmailCodeNotFound(request.emailCode));
        }

        Result result = ValidateEmailCode(foundCode);

        if (result.IsFailure) return result;

        foundCode.IsUsed = true;

        _emailCodeRepository.Update(foundCode);

        foundCode.AddEvent(new WelcomeMesageEvent(foundCode.User.Username,foundCode.User.Email));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static Result ValidateEmailCode(EmailCode emailCode)
       => emailCode switch
       {
           { IsUsed   : true } => Result.Failure(EmailCodeErrors.EmailCodeAlreadyUsed(emailCode.Code)),
           { IsRevoked: true } => Result.Failure(EmailCodeErrors.EmailCodeAlreadyRevoked(emailCode.Code)),
           { IsExpired: true } => Result.Failure(EmailCodeErrors.EmailCodeAlreadyExpired(emailCode.Code)),
           _ => Result.Success(),
       };
}
