namespace Ecommerce.Domain.UnitTest.ValueObjects;

public class EmailTests
{
    [Fact]
    public void ShouldReturnEmailIsNotValid()
    {
        string email = "#0000";

        bool isEmailValid = Email.IsValid(email, out _);

        Assert.False(isEmailValid);
    }

    [Fact]
    public void ShouldReturnEmailIsValid()
    {
        string email = "some@mail.com";

        bool isValid = Email.IsValid(email, out _ );

        Assert.True(isValid);
    }

    [Fact]
    public void InvalidEmailShouldReturnNull()
    {
        string email = string.Empty;

        Email.IsValid(email, out Result<Email>? validEmail);

        Assert.Null(validEmail);
    }

    [Fact]

    public void ValidEmailShouldNotReturnNull()
    {
        string email = "email@mail.com";

        Email.IsValid(email, out Result<Email>? validEmail);

        Assert.NotNull(validEmail);
    }
}
