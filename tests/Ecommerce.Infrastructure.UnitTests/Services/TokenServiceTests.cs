namespace Ecommerce.Infrastructure.UnitTests.Services;

using Options = Microsoft.Extensions.Options.Options;

public class TokenServiceTests
{
    private readonly ILogger<TokenService> _logger;
    private readonly IOptions<JwtOptions> _options;
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        _logger = Substitute.For<ILogger<TokenService>>();
        _options = Options.Create(new JwtOptions
        {
            SecretKey = "thisismysuperultrasecretkey00123456789",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = "audience",
            ValidIssuer = "issuer"
        });
        _tokenService = new TokenService(_logger, _options);
    }

    [Fact]
    public void GenerateRefreshToken_ValidRefreshToken_ReturnUniqueRefreshToken()
    {
        // Act
        string refreshToken1 = _tokenService.GenerateRefreshToken();

        string refreshToken2 = _tokenService.GenerateRefreshToken();

        // Assert
        Assert.NotEqual(refreshToken1,refreshToken2);
    }

    [Fact]
    public void GenerateToken_ValidUser_ReturnsToken()
    {
        // Arrange       
        Email.IsValid("sebastian@gmail.com", out var validEmail);

        Username.IsValid("szxro", out var validUsername);

        User user = new User
        {
            Id = 1,
            Username = validUsername!.Value,
            Email = validEmail!.Value
        };

        // Act
        string token = _tokenService.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.IsType<string>(token);
    }
}
