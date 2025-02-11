using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.Strategies;

public class EmailCodeExpirationStrategy : IExpiredStrategy<EmailCode>
{
    public EmailCodeExpirationStrategy()
    {
        
    }

    public Task<List<EmailCode>> GetExpiredEntitiesAsync(
        DateTime currentDateTime,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void MarkEntitiesAsExpired(List<EmailCode> entities)
    {
        throw new NotImplementedException();
    }
}