namespace Ecommerce.Infrastructure.UnitTests.Services;

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

    private static TokenService MakeTokenService()
    {
        return new TokenService(
            Substitute.For<ILogger<TokenService>>(),
            Options.Create(
                new JwtOptions()
            {
                SecretKey = "mysecretkey",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = "audience",
                ValidIssuer = "issuer"
            }));
    }
}
