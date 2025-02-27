using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IEmailCodeRepository : IRepositoryWriter<EmailCode>
{
    Task<EmailCode?> GetEmailCodeByEmailCode(string emailCode, CancellationToken cancellationToken = default);

    Task<bool> IsUserEmailCodeUsedByUsernameAsync(string username,CancellationToken cancellationToken = default);

    Task<EmailCode?> GetCurrentActiveEmailCodeByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
