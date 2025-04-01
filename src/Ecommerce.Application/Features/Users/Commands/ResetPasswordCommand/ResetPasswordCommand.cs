using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;

namespace Ecommerce.Application.Features.Users.Commands.ResetPasswordCommand;

public record ResetPasswordCommand(string resetCode, string newPassword) : ICommand;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
{
    private readonly IEmailCodeRepository _emailCodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly ICredentialsRepository _credentialsRepository;

    public ResetPasswordCommandHandler(
        IEmailCodeRepository emailCodeRepository,
        IUnitOfWork unitOfWork,
        IHashService hashService,
        ICredentialsRepository credentialsRepository)
    {
        _emailCodeRepository = emailCodeRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _credentialsRepository = credentialsRepository;
    }
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        EmailCode? foundCode = await _emailCodeRepository.GetEmailCodeByEmailCodeAsync(request.resetCode, cancellationToken);

        if (foundCode is null)
        {
            return Result.Failure(EmailCodeErrors.EmailCodeNotFound(request.resetCode));
        }

        Result result = ValidateEmailCode(foundCode);

        if (result.IsFailure) return result;

        foundCode.IsUsed = true;

        _emailCodeRepository.Update(foundCode);

        Credentials? foundCredentials = await _credentialsRepository.GetCredentialsByUsernameAsync(foundCode.User.Username.Value, cancellationToken);

        if (foundCredentials is null)
        {
            return Result.Failure(CredentialsErrors.CredentialsNotFoundByUsername(foundCode.User.Username.Value));
        }

        foundCredentials.IsActive = false;

        _credentialsRepository.Update(foundCredentials);

        (string hash, byte[] salt) = _hashService.GetHashAndSalt(request.newPassword);

        Credentials newCredentials = new Credentials
        {
            HashValue = hash,
            SaltValue = Convert.ToHexString(salt),
            IsActive = true,
            User = foundCode.User
        };

        _unitOfWork.ChangeTrackerToUnchanged(foundCode.User);

        _credentialsRepository.Add(newCredentials);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static Result ValidateEmailCode(EmailCode emailCode)
       => emailCode switch
       {
           { IsUsed: true } => Result.Failure(EmailCodeErrors.EmailCodeAlreadyUsed(emailCode.Code)),
           { IsRevoked: true } => Result.Failure(EmailCodeErrors.EmailCodeAlreadyRevoked(emailCode.Code)),
           { IsExpired: true } => Result.Failure(EmailCodeErrors.EmailCodeAlreadyExpired(emailCode.Code)),
           _ => Result.Success(),
       };
}
