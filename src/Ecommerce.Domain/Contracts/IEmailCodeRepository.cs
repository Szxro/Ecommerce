using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface IEmailCodeRepository : IRepositoryWriter<EmailCode>
{
    Task<EmailCode?> GetEmailCodeByEmailCode(string emailCode, CancellationToken cancellationToken = default);
}
