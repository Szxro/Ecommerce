using Microsoft.Extensions.DependencyInjection;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

[Inject(ServiceLifetime.Scoped)]
public class TemplateRepository
    : GenericRepository<Template>, ITemplateRepository
{
    public TemplateRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Template?> GetActiveTemplateByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Template
                                  .AsNoTracking()
                                  .Where(x => x.TemplateCategory.Name == category && x.IsActive)
                                  .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Template?> GetDefaultTemplateByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Template
                                  .AsNoTracking()
                                  .Where(x => x.TemplateCategory.Name == category && x.IsDefault)
                                  .FirstOrDefaultAsync(cancellationToken);
    }
}
