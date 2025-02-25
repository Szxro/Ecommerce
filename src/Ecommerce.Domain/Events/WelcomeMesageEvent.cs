using Ecommerce.Domain.ValueObjects;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.SharedKernel.Enums;

namespace Ecommerce.Domain.Events;

public class WelcomeMesageEvent : IDomainEvent
{
    public WelcomeMesageEvent(
        Username username,
        Email email)
    {
        UserName = username;
        Email = email;
    }

    public TemplateCategory Category => TemplateCategory.WelcomeMessage;

    public Username UserName { get; set; }

    public Email Email { get; set; }
}
