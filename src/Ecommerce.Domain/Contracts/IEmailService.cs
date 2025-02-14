using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Contracts;

public interface IEmailService
{
    string GenerateCode(int length = 10);

    Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);
}
