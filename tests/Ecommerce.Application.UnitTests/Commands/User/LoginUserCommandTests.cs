using Ecommerce.Application.Features.Users.Commands.LoginUserCommand;

namespace Ecommerce.Application.UnitTests.Commands.Users;

public class LoginUserCommandTests
{
	private readonly IUserRepository _userRepository;
	private readonly IHashService _hashService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ITokenService _tokenService;
	private readonly IRefreshTokenRepository _refreshTokenRepository;
	private readonly LoginUserCommandHandler _handler;

	private static readonly LoginUserCommand Command = new LoginUserCommand("username","somepassword");

	public LoginUserCommandTests()
	{
		_userRepository = Substitute.For<IUserRepository>();
		_hashService = Substitute.For<IHashService>();
		_unitOfWork = Substitute.For<IUnitOfWork>();
		_tokenService = Substitute.For<ITokenService>();
		_refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();

		_handler = new LoginUserCommandHandler(
            _userRepository,
            _unitOfWork,
            _hashService,
            _tokenService,
            _refreshTokenRepository);
	}

	[Fact]
	public async Task Handle_Should_ReturnsError_WhenUserIsNotFound()
	{
		_userRepository.GetUserByUsernameAsync("someusername").Returns((User?)null);

        Result<TokenResponse> result = await _handler.Handle(Command, CancellationToken.None);

		Assert.Equivalent(UserErrors.UserNotFoundByUsername, result.Error);
	}

	[Fact]
	public async Task Handle_Should_ReturnsError_WhenUserIsLockout()
	{
        User user = CreateUser("someusername", true, lockoutEnd: DateTime.UtcNow.AddMinutes(5));

		_userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        Result<TokenResponse> result = await _handler.Handle(Command, default);

		Assert.Equivalent(UserErrors.UserLockOut(user.Username.Value,user.LockOutEndAtUtc), result.Error);
	}

	[Fact]
	public async Task Handle_Should_ReturnError_WhenUserFailedCountIsBiggerOrEqualToThree()
	{
        User user = CreateUser("someusername", true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        Result<TokenResponse> result = await _handler.Handle(Command, default);

        Assert.Equivalent(UserErrors.UserLockOut(user.Username.Value, user.LockOutEndAtUtc), result.Error);
    }

	[Fact]
	public async Task Handle_Should_CallUserRepository_WhenUserFailedCountIsBiggerOrEqualToThree()
	{
        User user = CreateUser(Command.username, true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

		_userRepository.Received(1).Update(Arg.Is<User>(x => x.Username.Value == Command.username));
    }

	[Fact]
	public async Task Handle_Should_CallSaveChanges_WhenUserFailedCountIsBiggerOrEqualToThree()
	{
        User user = CreateUser("someusername", true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_LockOut_WhenUserFailedCountIsBiggerOrEqualtoThree()
    {
        User user = CreateUser("someusername", true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

        Assert.NotEqual(new DateTime(1999, 01, 01, 00, 00, 00), user.LockOutEndAtUtc);
    }

    [Fact]
    public async Task Handle_Should_ResetFailedCount_WhenUserFailedCountIsBiggerOrEqualtoThree()
    {
        User user = CreateUser("someusername", true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

        Assert.Equal(0, user.AccessFailedCount);
    }

    [Fact]
    public async Task Handle_Should_AddAnEvent_WhenTheUserIsLockout()
    {
        User user = CreateUser("someusername", true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

        Assert.NotEmpty(user.DomainEvent);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheCredentialsAreInvalid()
    {
        User user = CreateUser(Command.username,true);

        user.Credentials.Add(new Credentials {SaltValue = "124453",HashValue = "13214",IsActive = true });

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Command.password, user.Credentials.First().HashValue, user.Credentials.First().SaltValue).Returns(false);

        Result<TokenResponse> result = await _handler.Handle(Command, default);

        Assert.Equivalent(UserErrors.UserInvalidCredentials(user.AccessFailedCount),result.Error);
    }

    [Fact]
    public async Task Handle_Should_IncreaseFailCount_WhenTheCredentialsAreInvalid()
    {
        User user = CreateUser(Command.username, true);

        user.Credentials.Add(new Credentials { SaltValue = "124453", HashValue = "13214", IsActive = true });

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Command.password, user.Credentials.First().HashValue, user.Credentials.First().SaltValue).Returns(false);

        await _handler.Handle(Command, default);

        Assert.Equal(1, user.AccessFailedCount);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepository_WhenTheCredentialsAreInvalid()
    {
        User user = CreateUser(Command.username, true);

        user.Credentials.Add(new Credentials { SaltValue = "124453", HashValue = "13214", IsActive = true });

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Command.password, user.Credentials.First().HashValue, user.Credentials.First().SaltValue).Returns(false);

        await _handler.Handle(Command, default);

        _userRepository.Received(1).Update(Arg.Is<User>(x => x.Username.Value == Command.username));
    }

    [Fact]
    public async Task Handle_Should_CallSaveChanges_WhenTheCredentialsAreInvalid()
    {
        User user = CreateUser(Command.username, true);

        user.Credentials.Add(new Credentials { SaltValue = "124453", HashValue = "13214", IsActive = true });

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Command.password, user.Credentials.First().HashValue, user.Credentials.First().SaltValue).Returns(false);

        await _handler.Handle(Command, default);

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    private User CreateUser(string username, bool lockoutEnabled = false, int failedCount = 0, DateTime? lockoutEnd = null)
    {
        Username.IsValid(username, out Result<Username>? validUsername);

        return new User
        {
            FirstName = "Sebastian",
            LastName = "Vargas",
            Username = validUsername!.Value,
            LockOutEnabled = lockoutEnabled,
            AccessFailedCount = failedCount,
            LockOutEndAtUtc = lockoutEnd ?? new DateTime(1999, 01, 01, 00, 00, 00),           
        };
    }
}
