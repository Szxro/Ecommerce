namespace Ecommerce.Infrastructure.UnitTests.Services;

using Options = Microsoft.Extensions.Options.Options;

public class HashServiceTests
{
    private readonly IOptions<HashOptions> _options;
    private readonly HashService _hashService;

    public HashServiceTests()
    {
        // In this type of case you need to manually populate a IOptions<T>
        _options = Options.Create(new HashOptions
        {
            HashSize = 32,
            Iterations = 100000,
            SaltSize = 64
        });
        _hashService = new HashService(_options);
    }

    [Fact]
    public void GetHashAndSalt_ValidPassword_ReturnsTuple()
    {
        // Arrange
        string password = "Hello World 1234";

        // Act
        (string hash, byte[] salt) result = _hashService.GetHashAndSalt(password);

        // Assert
        Assert.IsType<(string hash, byte[] salt)>(result);
    }

    [Fact]
    public void VerifyHash_SamePassword_ReturnsTrue()
    {
        // Arrange
        string password = "Hello World 1234";

        (string hash, byte[] salt) = _hashService.GetHashAndSalt(password);

        // Act
        bool isValid = _hashService.VerifyHash(password, hash, Convert.ToHexString(salt));

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData("Hello_World_12345")]
    public void VerifyHash_DifferentPassword_ReturnsFalse(string differentPasssword)
    {
        // Arrange
        string password = "Hello World 1234";

        (string hash, byte[] salt) = _hashService.GetHashAndSalt(password);

        // Act
        bool isValid = _hashService.VerifyHash(differentPasssword, hash, Convert.ToHexString(salt));

        Assert.False(isValid);
    }
}