using Ecommerce.Domain.ValueObjects;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.SharedKernel.Enums;

namespace Ecommerce.Domain.Events.User;

public class SendEmailCodeEvent : IDomainEvent
{
    public SendEmailCodeEvent(
        Username username,
        string firstName,
        string lastName,
        Email email,
        string emailCode)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        EmailCode = emailCode;
    }

    public TemplateCategory Category => TemplateCategory.EmailCode;

    public Username Username { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public Email Email { get; }

    public string EmailCode { get; }
};