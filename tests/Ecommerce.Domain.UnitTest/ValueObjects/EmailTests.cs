namespace Ecommerce.Domain.UnitTest.ValueObjects;

public class EmailTests
{
    [Fact]
    public void IsValid_InvalidEmail_ReturnsFalse()
    {
        // Arrange
        string email = "#0000";

        // Act 
        bool isEmailValid = Email.IsValid(email, out _);

        // Assert
        Assert.False(isEmailValid);
    }

    [Fact]
    public void IsValid_NullEmail_ReturnsFalse()
    {
        string? email = null;

        bool isEmailValid = Email.IsValid(email, out _);

        Assert.False(isEmailValid);
    }

    [Fact]
    public void IsValid_EmptyEmail_ReturnsFalse()
    {
        string email = "";

        bool isValid = Email.IsValid(email, out _);

        Assert.False(isValid);
    }

    [Fact]
    public void IsValid_ValidEmail_ReturnsTrue()
    {
        string email = "some@mail.com";

        bool isValid = Email.IsValid(email, out _ );

        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_InvalidEmail_ReturnsNull()
    {
        string email = string.Empty;

        Email.IsValid(email, out Result<Email>? validEmail);

        Assert.Null(validEmail);
    }

    [Fact]
    public void IsValid_ValidEmail_ReturnsEmail()
    {
        string email = "email@mail.com";

        Email.IsValid(email, out Result<Email>? validEmail);

        Assert.IsType<Result<Email>>(validEmail);
    }
}
