using Ecommerce.Application.Features.RefreshTokens.Commands;
using System.Security.Claims;

namespace Ecommerce.Application.UnitTests.Commands.RefreshTokens;

public class RegenerateTokenCommandTests
{
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RegenerateTokenCommandHandler _handler;

    private static readonly RegenerateTokenCommand Command = new RegenerateTokenCommand("token","refreshToken");
    private static readonly ClaimsIdentity ExpiredClaims = new ClaimsIdentity([new Claim("name", "somename"), new Claim("exp", "1516239022")]);
    private static readonly ClaimsIdentity EmptyClaims = new ClaimsIdentity();
    private static readonly ClaimsIdentity InvalidTimeStampClaims = new ClaimsIdentity([new Claim("name", "somename"), new Claim("exp", "dasdasfqwe")]);
    private static readonly ClaimsIdentity StillValidClaims = new ClaimsIdentity([new Claim("name", "somename"), new Claim("exp", DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds().ToString())]);

    public RegenerateTokenCommandTests()
    {
        _tokenService = Substitute.For<ITokenService>();
        _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new RegenerateTokenCommandHandler(_tokenService,_refreshTokenRepository,_unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheTokenIsInvalid()
    {
        _tokenService.ValidateToken(Arg.Any<string>()).Returns((false,null));

        Result<TokenResponse> result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(TokenErrors.InvalidToken, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheTokenClaimsAreInvalid()
    {
        _tokenService.ValidateToken(Arg.Any<string>()).Returns((true, EmptyClaims));

        Result<TokenResponse> result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(TokenErrors.InvalidTokenClaims, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheTokenExpirateStampIsInvalid()
    {
        _tokenService
            .ValidateToken(Arg.Any<string>())
            .Returns((true, InvalidTimeStampClaims));

        Result<TokenResponse> result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(TokenErrors.InvalidTokenExpiryStamp, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheTokenIsStillValid()
    {
        _tokenService
            .ValidateToken(Arg.Any<string>())
            .Returns((true, StillValidClaims));

        Result<TokenResponse> result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(TokenErrors.TokenStillValid, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnsError_WhenTheGivenRefreshTokenDontExists()
    {
        _tokenService
            .ValidateToken(Arg.Any<string>())
            .Returns((true, ExpiredClaims));

        _refreshTokenRepository.GetValidRefreshTokenAsync(Arg.Any<string>(), Arg.Any<string>(), CancellationToken.None).Returns((RefreshToken?)null);

        Result<TokenResponse> result = await _handler.Handle(Command, CancellationToken.None);

        Assert.Equivalent(RefreshTokenErrors.InvalidRefreshToken, result.Error);
    }

    [Fact]
    public async Task Handle_Should_CallRefreshTokenRepository_WhenTheGivenRefreshTokenExists()
    {
        _tokenService
            .ValidateToken(Arg.Any<string>())
            .Returns((true, ExpiredClaims));

        _refreshTokenRepository.GetValidRefreshTokenAsync(Arg.Any<string>(), Arg.Any<string>(), CancellationToken.None).Returns(new RefreshToken { Token = "1234"});

        await _handler.Handle(Command, CancellationToken.None);

        _refreshTokenRepository.Received(1).Update(Arg.Any<RefreshToken>());

        _refreshTokenRepository.Received(1).Add(Arg.Any<RefreshToken>());
    }

    [Fact]
    public async Task Handle_Should_CallSaveChangesAsync_WhenTheGivenRefreshTokenExists()
    {
        _tokenService
            .ValidateToken(Arg.Any<string>())
            .Returns((true, ExpiredClaims));

        _refreshTokenRepository.GetValidRefreshTokenAsync(Arg.Any<string>(), Arg.Any<string>(), CancellationToken.None).Returns(new RefreshToken { Token = "1234" });

        await _handler.Handle(Command, CancellationToken.None);

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnsSucess_WhenTheTokenAndRefreshTokenAreValid()
    {
        _tokenService
            .ValidateToken(Arg.Any<string>())
            .Returns((true, ExpiredClaims));

        _refreshTokenRepository.GetValidRefreshTokenAsync(Arg.Any<string>(), Arg.Any<string>(), CancellationToken.None).Returns(new RefreshToken { Token = "1234" });

        Result<TokenResponse> result = await _handler.Handle(Command, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
