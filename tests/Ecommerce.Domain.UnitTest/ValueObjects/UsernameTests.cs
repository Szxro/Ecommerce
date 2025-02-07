namespace Ecommerce.Domain.UnitTest.ValueObjects;

public class UsernameTests
{
    [Fact]
    public void IsValid_InvalidUsername_ReturnsFalse()
    {
        string username = "@username_1235''";

        bool isValid = Username.IsValid(username, out _);

        Assert.False(isValid);
    }

    [Fact]
    public void IsValid_NullUsername_ReturnsFalse()
    {
        string? username = null;

        bool isValid = Username.IsValid(username, out _);

        Assert.False(isValid);
    }

    [Fact]
    public void IsValid_EmptyUsername_ReturnsFalse()
    {
        string username = "";

        bool isValid = Username.IsValid(username,out _);

        Assert.False(isValid);  
    }

    [Fact]
    public void IsValid_ValidUsername_ReturnsTrue()
    {
        string username = "username_12345";

        bool isValid = Username.IsValid(username, out _);

        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_ValidUsername_ReturnsUsername()
    {
        string username = "username_12345";

        Username.IsValid(username, out Result<Username>? validUsername);

        Assert.IsType<Result<Username>>(validUsername);
    }

    [Fact]
    public void IsValid_InvalidUsername_ReturnsNull()
    {
        string username = "@username_1235''";

        Username.IsValid(username, out Result<Username>? validUsername);

        Assert.Null(validUsername);
    }
}
