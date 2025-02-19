namespace Ecommerce.Infrastructure.UnitTests.Services;

using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using Options = Microsoft.Extensions.Options.Options;

public class TokenServiceTests
{
    [Fact]
    public void GenerateRefreshToken_ValidRefreshToken_ReturnUniqueRefreshToken()
    {
        // Arrange
        TokenService tokenService = MakeTokenService();

        // Act
        string refreshToken1 = tokenService.GenerateRefreshToken();

        string refreshToken2 = tokenService.GenerateRefreshToken();

        // Assert
        Assert.NotEqual(refreshToken1,refreshToken2);
    }

    [Fact]
    public void GenerateToken_ValidUser_ReturnsToken()
    {
        // Arrange
        TokenService tokenService = MakeTokenService();

        Email.IsValid("sebastian@gmail.com", out var validEmail);

        Username.IsValid("szxro", out var validUsername);

        User user = new User
        {
            Id = 1,
            Username = validUsername!.Value,
            Email = validEmail!.Value
        };

        // Act
        string token = tokenService.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.IsType<string>(token);
    }

    private static TokenService MakeTokenService()
    {
        return new TokenService(
            Substitute.For<ILogger<TokenService>>(),
            Options.Create(
                new JwtOptions()
            {
                SecretKey = "thisismysuperultrasecretkey00123456789",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = "audience",
                ValidIssuer = "issuer"
            }));
    }
}
