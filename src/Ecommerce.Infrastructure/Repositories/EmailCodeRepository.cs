using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class EmailCodeRepository : GenericRepository<EmailCode>, IEmailCodeRepository
{
    public EmailCodeRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<EmailCode?> GetEmailCodeByEmailCode(string emailCode,CancellationToken cancellationToken = default)
    {
        return await _appDbContext.EmailCode.FirstOrDefaultAsync(x => x.Code == emailCode,cancellationToken);
    }
}
