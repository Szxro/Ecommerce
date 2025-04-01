using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Events;
using TemplateCategory = Ecommerce.SharedKernel.Enums.TemplateCategory;

namespace Ecommerce.Application.Features.Users.Commands.RequestPasswordResetCommand;

public record RequestPasswordResetCommand(string username) : ICommand;

public class RequestPasswordResetCommandHandler : ICommandHandler<RequestPasswordResetCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailCodeRepository _emailCodeRepository;

    public RequestPasswordResetCommandHandler(
        IUserRepository userRepository,
        IEmailService emailService,
        IUnitOfWork unitOfWork,
        IEmailCodeRepository emailCodeRepository)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _emailCodeRepository = emailCodeRepository;
    }
    public async Task<Result> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.username) || string.IsNullOrWhiteSpace(request.username))
        {
            return Result.Failure(UserErrors.UsernameNotUniqueOrInvalid);
        }

        User? currentUser = await _userRepository.GetUserByUsernameAsync(request.username, cancellationToken);

        if (currentUser is null)
        {
            return Result.Failure(UserErrors.UserNotFoundByUsername);
        }

        string emailCode = _emailService.GenerateCode();

        EmailCode newEmailCode = new EmailCode
        {
            Code = emailCode,
            User = currentUser
        };        

        newEmailCode.AddEvent(new TemplateRenderEvent(
            TemplateCategory.ResetPasswordEmail,
            new 
            {
                Username = currentUser.Username.Value,                
                ResetCode = emailCode
            },
            currentUser.Email));

        _unitOfWork.ChangeTrackerToUnchanged(newEmailCode.User);

        _emailCodeRepository.Add(newEmailCode);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
