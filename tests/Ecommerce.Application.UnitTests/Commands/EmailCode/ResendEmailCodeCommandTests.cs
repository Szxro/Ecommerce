using Ecommerce.Application.Features.EmailCodes.Commands.ResendEmailCode;

namespace Ecommerce.Application.UnitTests.Commands.EmailCodes;

public class ResendEmailCodeCommandTests
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailCodeRepository _emailCodeRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ResendEmailCodeCommandHandler _handler;

    private static readonly ResendEmailCodeCommand Command = new ResendEmailCodeCommand("someusername");
    public ResendEmailCodeCommandTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _emailCodeRepository = Substitute.For<IEmailCodeRepository>();
        _emailService = Substitute.For<IEmailService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new ResendEmailCodeCommandHandler(_userRepository, _emailCodeRepository, _emailService, _unitOfWork);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Handle_Should_ReturnFailure_WhenTheUsernameIsEmptyOrNull(string? username)
    {
        ResendEmailCodeCommand command = Command with { username = username! };

        Result result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheUserIsNotFound()
    {
        _userRepository.GetUserByUsernameAsync(Arg.Any<string>()).Returns((User?)null);

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(UserErrors.UserNotFoundByUsername,result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenTheUserIsAlreadyVerified()
    {
        _userRepository.GetUserByUsernameAsync(Arg.Any<string>()).Returns(CreateUser(isEmailVerified: true));

        _emailCodeRepository.IsUserEmailCodeUsedByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(true);

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(EmailCodeErrors.UserAlreadyVerified(Command.username),result.Error);
    }

    [Fact]
    public async Task Handle_Should_CallEmailCodeRepository_WhenAnEmailCodeIsFound()
    {
        EmailCode emailCode = new EmailCode { Code = "123455" };

        _userRepository.GetUserByUsernameAsync(Arg.Any<string>()).Returns(CreateUser());

        _emailCodeRepository.IsUserEmailCodeUsedByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(false);

        _emailCodeRepository.GetCurrentActiveEmailCodeByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(emailCode);

        await _handler.Handle(Command, CancellationToken.None);

        Assert.True(emailCode.IsRevoked);

        _emailCodeRepository.Received(1).Update(Arg.Any<EmailCode>());
    }

    [Fact]
    public async Task Handle_Should_CallEmailCodeRepository_WhenCreatingANewEmailCode()
    {
        _userRepository.GetUserByUsernameAsync(Arg.Any<string>()).Returns(CreateUser());

        _emailCodeRepository.IsUserEmailCodeUsedByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(false);

        _emailCodeRepository.GetCurrentActiveEmailCodeByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(new EmailCode { });

        await _handler.Handle(Command, CancellationToken.None);

        _emailCodeRepository.Received(1).Add(Arg.Any<EmailCode>());
    }

    [Fact]
    public async Task Handle_Should_CallSaveChanges_WhenCreatingANewEmailCode()
    {
        _userRepository.GetUserByUsernameAsync(Arg.Any<string>()).Returns(CreateUser());

        _emailCodeRepository.IsUserEmailCodeUsedByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(false);

        _emailCodeRepository.GetCurrentActiveEmailCodeByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(new EmailCode { });

        await _handler.Handle(Command, CancellationToken.None);

        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_Should_ReturnsSucess_WhenCreatingANewEmailCode()
    {
        _userRepository.GetUserByUsernameAsync(Arg.Any<string>()).Returns(CreateUser());

        _emailCodeRepository.IsUserEmailCodeUsedByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(false);

        _emailCodeRepository.GetCurrentActiveEmailCodeByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(new EmailCode { });

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    private User CreateUser(bool isEmailVerified = false, string? username = null)
    {
        Email.IsValid("someemail@gmail.com", out Result<Email>? validEmail);

        Username.IsValid(username ?? Command.username, out Result<Username>? validUsername);

        return new User
        {
            FirstName = "somefirstname",
            LastName = "somelastname",
            Username = validUsername!.Value,
            Email = validEmail!.Value,
            IsEmailVerified = isEmailVerified
        };
    }
}
