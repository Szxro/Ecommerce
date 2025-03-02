using Ecommerce.Application.Utilities;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Events;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Application.EventHandlers;

class TemplateRenderEventHandler : IDomainEventHandler<TemplateRenderEvent>
{
    private readonly IEmailService _emailService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TemplateRenderEventHandler> _logger;

    public TemplateRenderEventHandler(
        IEmailService emailService,
        IServiceScopeFactory scopeFactory,
        ILogger<TemplateRenderEventHandler> logger)
    {
        _emailService = emailService;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    public async Task Handle(TemplateRenderEvent notification, CancellationToken cancellationToken)
    {        
        _logger.LogInformation(
            "Sending an email with the template category: '{category}' for an email: '{email}'",
            notification.Category.ToString(),
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

        string render = ApplicationUtilities.TemplateRender(source.Content, notification.Model);

        await _emailService.SendEmailAsync(
            new EmailMessage
            {
                Subject = source.Title,
                Body = render,
                ToAddress = notification.Email.Value
            });
    }
}
