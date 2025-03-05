using Ecommerce.Application.Features.EmailCodes.Commands.VerifyEmailCode;

namespace Ecommerce.Application.UnitTests.Commands.EmailCodes;

public class VerifyEmailCommandTests
{
    private readonly IEmailCodeRepository _emailCodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly VerifyEmailCodeCommandHandler _handler;

    private static readonly VerifyEmailCodeCommand Command = new VerifyEmailCodeCommand("123456");
    public VerifyEmailCommandTests()
    {
        _emailCodeRepository = Substitute.For<IEmailCodeRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new VerifyEmailCodeCommandHandler(_emailCodeRepository, _unitOfWork);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Handle_Should_ReturnsFailure_WhenTheEmailCodeIsEmptyOrNull(string? emailCode)
    {
        VerifyEmailCodeCommand command = Command with { emailCode = emailCode! };

        Result result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheEmailCodeIsNotFound()
    {
        _emailCodeRepository.GetEmailCodeByEmailCodeAsync(Arg.Any<string>(), CancellationToken.None).Returns((EmailCode?)null);

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(EmailCodeErrors.EmailCodeNotFound(Command.emailCode), result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheEmailCodeHaveRevokedState()
    {
        _emailCodeRepository.GetEmailCodeByEmailCodeAsync(Arg.Any<string>(), CancellationToken.None).Returns(CreateEmailCode(isRevoked:true));

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(EmailCodeErrors.EmailCodeAlreadyRevoked(Command.emailCode),result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheEmailCodeHaveUsedState()
    {
        _emailCodeRepository.GetEmailCodeByEmailCodeAsync(Arg.Any<string>(), CancellationToken.None).Returns(CreateEmailCode(isUsed:true));

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(EmailCodeErrors.EmailCodeAlreadyUsed(Command.emailCode), result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheEmailCodeHaveExpiredState()
    {
        _emailCodeRepository.GetEmailCodeByEmailCodeAsync(Arg.Any<string>(), CancellationToken.None).Returns(CreateEmailCode(isExpired: true));

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(EmailCodeErrors.EmailCodeAlreadyExpired(Command.emailCode), result.Error);
    }

    [Fact]
    public async Task Handle_Should_CallEmailCodeRepository_WhenTheEmailCodeIsValid()
    {
        _emailCodeRepository.GetEmailCodeByEmailCodeAsync(Arg.Any<string>(), CancellationToken.None).Returns(CreateEmailCode());

        await _handler.Handle(Command, CancellationToken.None);

        _emailCodeRepository.Received(1).Update(Arg.Any<EmailCode>());
    }

    [Fact]
    public async Task Handle_Should_CallSaveChanges_WhenTheEmailCodeIsValid()
    {
        _emailCodeRepository.GetEmailCodeByEmailCodeAsync(Arg.Any<string>(), CancellationToken.None).Returns(CreateEmailCode());

        await _handler.Handle(Command, CancellationToken.None);

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnsSuccess_WhenTheEmailCodeIsValid()
    {
        _emailCodeRepository.GetEmailCodeByEmailCodeAsync(Arg.Any<string>(), CancellationToken.None).Returns(CreateEmailCode());

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    private EmailCode CreateEmailCode(bool isRevoked = false, bool isUsed = false,bool isExpired = false,string? emailCode = null)
    {
        Username.IsValid("someusername", out Result<Username>? validUsername);

        Email.IsValid("someemail@gmail.com", out Result<Email>? validEmail);

        return new EmailCode
        {
            Code = emailCode ?? Command.emailCode,
            IsRevoked = isRevoked,
            IsUsed = isUsed,
            IsExpired = isExpired,
            User = new User
            {
                Username = validUsername!.Value,
                Email = validEmail!.Value
            }
        };
    }
}
