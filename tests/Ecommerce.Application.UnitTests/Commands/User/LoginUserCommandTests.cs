using Ecommerce.Application.Features.Users.Commands.LoginUserCommand;
using Ecommerce.Application.UnitTests.Helpers;

namespace Ecommerce.Application.UnitTests.Commands.Users;

public class LoginUserCommandTests : IDisposable // Context Clean Up
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

        _handler = new LoginUserCommandHandler(_userRepository,
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
        User user = CreateUser(true, lockoutEnd: DateTime.UtcNow.AddMinutes(5));

		_userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        Result<TokenResponse> result = await _handler.Handle(Command, default);

		Assert.Equivalent(UserErrors.UserLockOut(user.Username.Value,user.LockOutEndAtUtc), result.Error);
	}

	[Fact]
	public async Task Handle_Should_ReturnsError_WhenUserFailedCountIsBiggerOrEqualToThree()
	{
        User user = CreateUser(true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        Result<TokenResponse> result = await _handler.Handle(Command, default);

        Assert.Equivalent(UserErrors.UserLockOut(user.Username.Value, user.LockOutEndAtUtc), result.Error);
    }

	[Fact]
	public async Task Handle_Should_CallUserRepository_WhenUserFailedCountIsBiggerOrEqualToThree()
	{
        User user = CreateUser(true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

		_userRepository.Received(1).Update(Arg.Is<User>(x => x.Username.Value == Command.username));
    }

	[Fact]
	public async Task Handle_Should_CallSaveChanges_WhenUserFailedCountIsBiggerOrEqualToThree()
	{
        User user = CreateUser(true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_LockOut_WhenUserFailedCountIsBiggerOrEqualtoThree()
    {
        SystemTimeUtc.Set(new DateTime(1999, 01, 01, 00, 00, 00));

        User user = CreateUser(true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

        Assert.NotEqual(SystemTimeUtc.Now, user.LockOutEndAtUtc);
    }

    [Fact]
    public async Task Handle_Should_ResetFailedCount_WhenUserFailedCountIsBiggerOrEqualtoThree()
    {
        User user = CreateUser(true, 3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

        Assert.Equal(0, user.AccessFailedCount);
    }

    [Fact]
    public async Task Handle_Should_AddAnEvent_WhenTheUserIsLockout()
    {
        User user = CreateUser(true,3);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        await _handler.Handle(Command, default);

        Assert.NotEmpty(user.DomainEvent);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheCredentialsAreInvalid()
    {
        User user = CreateUser(true);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        Result<TokenResponse> result = await _handler.Handle(Command, default);

        Assert.Equivalent(UserErrors.UserInvalidCredentials(user.AccessFailedCount),result.Error);
    }

    [Fact]
    public async Task Handle_Should_IncreaseFailCount_WhenTheCredentialsAreInvalid()
    {
        User user = CreateUser(true);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>() ).Returns(false);

        await _handler.Handle(Command, default);

        Assert.Equal(1, user.AccessFailedCount);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepository_WhenTheCredentialsAreInvalid()
    {
        User user = CreateUser(true);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        await _handler.Handle(Command, default);

        _userRepository.Received(1).Update(Arg.Is<User>(x => x.Username.Value == Command.username));
    }

    [Fact]
    public async Task Handle_Should_CallSaveChanges_WhenTheCredentialsAreInvalid()
    {
        User user = CreateUser(true);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(false);
        
        await _handler.Handle(Command, default);

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_MarkAsRevoked_WhenARefreshTokenIsFound()
    {
        User user = CreateUser(true);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        _refreshTokenRepository.GetUnusedUserRefreshTokenByUsernameAsync(Arg.Any<string>(), CancellationToken.None).Returns(new RefreshToken { });

        await _handler.Handle(Command, default);

        _refreshTokenRepository.Received(1).Update(Arg.Any<RefreshToken>());
    }

    [Fact]
    public async Task Handle_Should_ResetFailedAttempts_WhenUserIsValid()
    {
        User user = CreateUser(true);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        await _handler.Handle(Command, default);

        _userRepository.Received(1).Update(Arg.Any<User>());

        Assert.Equal(0, user.AccessFailedCount);
    }

    [Fact]
    public async Task Handle_Should_CallRefreshTokenRepository_WhenUserIsValid()
    {
        User user = CreateUser(true);
       
        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        await _handler.Handle(Command, default);

        _refreshTokenRepository.Received(1).Add(Arg.Any<RefreshToken>());
    }

    [Fact]
    public async Task Handle_Should_CallSaveChanges_WhenUserIsValid()
    {
        User user = CreateUser(true);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        await _handler.Handle(Command, default);

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnsSucess_WhenUserIsValid()
    {
        User user = CreateUser(true);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        Result<TokenResponse> result = await _handler.Handle(Command, default);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Should_ReturnsToken_WhenUserLogsInSuccessfully()
    {
        User user = CreateUser(true);

        _userRepository.GetUserByUsernameAsync(Arg.Is<string>(e => e == Command.username)).Returns(user);

        _hashService.VerifyHash(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        _tokenService.GenerateToken(Arg.Any<User>()).Returns("12345");

        _tokenService.GenerateRefreshToken().Returns("6788");

        Result<TokenResponse> result = await _handler.Handle(Command, CancellationToken.None);

        Assert.NotNull(result.Value);
    }

    private User CreateUser(bool lockoutEnabled = false, int failedCount = 0, DateTime? lockoutEnd = null, string? username = null)
    {
        Username.IsValid(username ?? Command.username, out Result<Username>? validUsername);

        SystemTimeUtc.Set(lockoutEnd ?? new DateTime(1999, 01, 01, 00, 00, 00));

        return new User
        {
            FirstName = "Sebastian",
            LastName = "Vargas",
            Credentials = new HashSet<Credentials> { new Credentials { HashValue = "", SaltValue = "", IsActive = true } },
            Username = validUsername!.Value,
            LockOutEnabled = lockoutEnabled,
            AccessFailedCount = failedCount,
            LockOutEndAtUtc = SystemTimeUtc.Now,
        };
    }

    public void Dispose()
    {
        // Reset date at the end of every of each test
        SystemTimeUtc.Reset();
    }
}
