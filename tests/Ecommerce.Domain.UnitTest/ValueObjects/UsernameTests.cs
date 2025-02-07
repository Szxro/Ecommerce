namespace Ecommerce.Domain.UnitTest.ValueObjects;

public class UsernameTests
{
    [Fact]
    public void ShouldReturnInvalidUsername()
    {
        string username = "@username_1235''";

        bool isValid = Username.IsValid(username, out _);

        Assert.False(isValid);
    }

    [Fact]

    public void InvalidUsernameShouldReturnNull()
    {
        string username = "@username_1235''";

        Username.IsValid(username, out Result<Username>? validUsername);

        Assert.Null(validUsername);
    }

    [Fact]

    public void ShouldReturnUsernameValid()
    {
        string username = "username_12345";

        bool isValid = Username.IsValid(username, out _);

        Assert.True(isValid);
    }

    [Fact]
    public void ValidUsernameShouldNotReturnNull()
    {
        string username = "username_12345";

        Username.IsValid(username, out Result<Username>? validUsername);

        Assert.NotNull(validUsername);
    }
}
