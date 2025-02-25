using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;

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
        EmailCode? foundCode = await _emailCodeRepository.GetEmailCodeByEmailCode(request.emailCode);

        if (foundCode is null)
        {
            return Result.Failure(EmailCodeErrors.EmailCodeNotFound(request.emailCode));
        }

        Result result = ValidateEmailCode(foundCode);

        if (result.IsFailure) return result;

        foundCode.IsUsed = true;

        _emailCodeRepository.Update(foundCode);

        // TODO: Welcome message event

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
