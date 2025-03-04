using Ecommerce.Application.Features.Users.Commands.RegisterUserCommand;

namespace Ecommerce.Application.UnitTests.Commands.Users;

public class RegisterUserCommandTests
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RegisterUserCommandHandler _handler;

    private readonly RegisterUserCommand Command = new RegisterUserCommand("luis", "vargas", "lvasgas","someemail@email.com", "1234");

    public RegisterUserCommandTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _hashService = Substitute.For<IHashService>();
        _emailService = Substitute.For<IEmailService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new RegisterUserCommandHandler(_userRepository, _hashService, _emailService, _unitOfWork);
    }

    [Theory]
    [InlineData("someemail.com")]
    [InlineData("someeeema...il.com")]
    public async Task Handle_Should_ReturnError_WhenEmailIsInvalid(string email)
    {
        RegisterUserCommand command = Command with { email = email };

        Result result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equivalent(UserErrors.EmailNotUniqueOrInvalid, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsNotUnique()
    {
        _userRepository.IsEmailNotUniqueAsync(Arg.Is<string>(e => e == Command.email)).Returns(true);

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(UserErrors.EmailNotUniqueOrInvalid, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenUsernameIsInvalid()
    {
        RegisterUserCommand command = Command with { username = "" };

        Result result = await _handler.Handle(command, default);

        Assert.Equivalent(UserErrors.UsernameNotUniqueOrInvalid, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenUsernameIsNotUnique()
    {
        _userRepository.IsUsernameNotUniqueAsync(Arg.Is<string>(e => e == Command.username)).Returns(true);

        Result result = await _handler.Handle(Command, default);

        Assert.Equivalent(UserErrors.UsernameNotUniqueOrInvalid, result.Error);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepository_WhenUserIsValid()
    {
        _hashService.GetHashAndSalt(Arg.Is<string>(e => e == Command.password)).Returns(("1234", []));

        _emailService.GenerateCode().Returns("1234");

        Result result = await _handler.Handle(Command, CancellationToken.None);

        _userRepository.Received(1).Add(Arg.Is<User>(user => user.Email.Value == Command.email));
    }

    [Fact]
    public async Task Handle_Should_CallSaveChanges_WhenUserIsValid()
    {
        _hashService.GetHashAndSalt(Arg.Is<string>(e => e == Command.password)).Returns(("1234", []));

        _emailService.GenerateCode().Returns("1234");

        await _handler.Handle(Command, CancellationToken.None);

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenUserIsValid()
    {
        _hashService.GetHashAndSalt(Arg.Is<string>(e => e == Command.password)).Returns(("1234", []));

        _emailService.GenerateCode().Returns("1234");

        Result result = await _handler.Handle(Command, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
