using Ecommerce.Application.Utilities;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Events;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Application.EventHandlers;

// TODO: REFACTOR REPETITIVE LOGIC
class WelcomeMesageEventHandler : IDomainEventHandler<WelcomeMesageEvent>
{
    private readonly IEmailService _emailService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WelcomeMesageEventHandler> _logger;

    public WelcomeMesageEventHandler(
        IEmailService emailService,
        IServiceScopeFactory scopeFactory,
        ILogger<WelcomeMesageEventHandler> logger)
    {
        _emailService = emailService;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    public async Task Handle(WelcomeMesageEvent notification, CancellationToken cancellationToken)
    {        
        _logger.LogInformation(
            "Sending a email code for a user with a username '{username}' and with an email '{email}'",
            notification.UserName.Value,
            notification.Email.Value);
       
        using IServiceScope scope = _scopeFactory.CreateScope();

        string templateCategory = ApplicationUtilities.GetTemplateCategoryDescription(notification.Category);

        ITemplateRepository templateRepository = scope.ServiceProvider.GetRequiredService<ITemplateRepository>();

        Template? source = await templateRepository.GetActiveTemplateByCategoryAsync(templateCategory, cancellationToken)
            ?? await templateRepository.GetDefaultTemplateByCategoryAsync(templateCategory, cancellationToken);

        if (source is null)
        {
            _logger.LogError(
                "No active or default templates was found for category: {templateCategory}",
                templateCategory);

            return;
        }

        object model = new
        {
            Username = notification.UserName.Value
        };

        string render = ApplicationUtilities.TemplateRender(source.Content, model);

        await _emailService.SendEmailAsync(
            new EmailMessage
            {
                Subject = source.Title,
                Body = render,
                ToAddress = notification.Email.Value
            });
    }
}
