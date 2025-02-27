using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Events;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Category = Ecommerce.SharedKernel.Enums.TemplateCategory;

namespace Ecommerce.Application.Features.EmailCodes.Commands.ResendEmailCode;

public record ResendEmailCodeCommand(string username) : ICommand;

public class ResendEmailCodeCommandHandler : ICommandHandler<ResendEmailCodeCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailCodeRepository _emailCodeRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public ResendEmailCodeCommandHandler(
        IUserRepository userRepository,
        IEmailCodeRepository emailCodeRepository,
        IEmailService emailService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _emailCodeRepository = emailCodeRepository;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(ResendEmailCodeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.username) || string.IsNullOrWhiteSpace(request.username))
        {
            return Result.Failure(Error.Validation("The username cant be empty or null"));
        }

        User? foundUser = await _userRepository.GetUserByUsernameAsync(request.username, cancellationToken);

        if (foundUser is null)
        {
            return Result.Failure(UserErrors.UserNotFoundByUsername);
        }

        if (await _emailCodeRepository.IsUserEmailCodeUsedByUsernameAsync(request.username, cancellationToken))
        {
            return Result.Failure(EmailCodeErrors.UserAlreadyVerified(request.username));
        }

        EmailCode? activeEmailCode = await _emailCodeRepository.GetCurrentActiveEmailCodeByUsernameAsync(request.username, cancellationToken);

        if (activeEmailCode is not null)
        {
            activeEmailCode.IsRevoked = true;

            _emailCodeRepository.Update(activeEmailCode);
        }

        string emailCode = _emailService.GenerateCode();

        EmailCode newEmailCode = new EmailCode
        {
            Code = emailCode,
            User = foundUser
        };

        _unitOfWork.ChangeTrackerToUnchanged(newEmailCode.User);

        _emailCodeRepository.Add(newEmailCode);

        foundUser.AddEvent(
         new TemplateRenderEvent(
             Category.EmailCode,
             new
             {
                 foundUser.FirstName,
                 foundUser.LastName,
                 Email = foundUser.Email.Value,
                 EmailCode = emailCode
             },
             foundUser.Email
         ));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
