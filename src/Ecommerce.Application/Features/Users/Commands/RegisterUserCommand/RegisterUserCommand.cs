using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Events;
using Ecommerce.Domain.ValueObjects;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Category = Ecommerce.SharedKernel.Enums.TemplateCategory;

namespace Ecommerce.Application.Features.Users.Commands.RegisterUserCommand;

public record RegisterUserCommand(
    string firstName,
    string lastName,
    string username,
    string email,
    string password) : ICommand;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IHashService hashService,
        IEmailService emailService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _hashService = hashService;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (!Email.IsValid(request.email,out Result<Email>? validEmail) 
                || await _userRepository.IsEmailNotUniqueAsync(request.email, cancellationToken))
        {
            return Result.Failure(UserErrors.EmailNotUniqueOrInvalid);
        }

        if (!Username.IsValid(request.username,out Result<Username>? validUsername) 
                || await _userRepository.IsUsernameNotUniqueAsync(request.username, cancellationToken))
        {
            return Result.Failure(UserErrors.UsernameNotUniqueOrInvalid);
        }

        User newUser = new User
        {
            FirstName = request.firstName,
            LastName = request.lastName,
            Email = validEmail!.Value,
            Username = validUsername!.Value
        };

        (string hash,byte[] salt) = _hashService.GetHashAndSalt(request.password);

        newUser.Credentials.Add(
            new Credentials
            {
                HashValue = hash,
                SaltValue = Convert.ToHexString(salt),
                IsActive = true
            });

        string emailCode = _emailService.GenerateCode();

        newUser.EmailCodes.Add(
            new EmailCode
            {
                Code = emailCode,
            });

        newUser.AddEvent(
            new TemplateRenderEvent(
                Category.EmailCode,
                new
                { 
                    newUser.FirstName,
                    newUser.LastName,
                    Email = newUser.Email.Value,
                    EmailCode = emailCode
                },
                newUser.Email
            )
        );

        _userRepository.Add(newUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
