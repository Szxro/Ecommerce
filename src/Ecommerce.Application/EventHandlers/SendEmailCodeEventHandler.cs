using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Events.User;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.SharedKernel.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fluid;
using Ecommerce.Application.Utilities;

namespace Ecommerce.Application.Features.Users.EventHandlers;

public class SendEmailCodeEventHandler : IDomainEventHandler<SendEmailCodeEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailCodeEventHandler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    private static readonly FluidParser _parser = new FluidParser();

    public SendEmailCodeEventHandler(
        IEmailService emailService,
        ILogger<SendEmailCodeEventHandler> logger,
        IServiceScopeFactory scopeFactory)
    {
        _emailService = emailService;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task Handle(SendEmailCodeEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Sending a email code for a user with a username '{username}' and with an email '{email}'",
            notification.Username.Value,
            notification.Email.Value);

        // Need to create a scope, because you are using a service outside of the current scope of the application
        using IServiceScope scope = _scopeFactory.CreateScope();

        ITemplateRepository templateRepository = scope.ServiceProvider.GetRequiredService<ITemplateRepository>();

        Template? source = await templateRepository.GetActiveTemplateByCategoryAsync(notification.Category, cancellationToken)
            ?? await templateRepository.GetDefaultTemplateByCategoryAsync(notification.Category, cancellationToken);

        if (source is null)
        {
            _logger.LogError(
                "No active or default templates was found for category: {templateCategory}",
                notification.Category);

            return; 
        }

        object model = new 
        {
            notification.FirstName,
            notification.LastName,
            Email = notification.Email.Value,
            notification.EmailCode,
        };

        string render = TemplateRender.Render(source.Content, model);

        await _emailService.SendEmailAsync(
            new EmailMessage
        {
            Subject = source.Title,
            Body = render,
            ToAddress = notification.Email.Value
        });        
    }
}
