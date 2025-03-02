using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Contracts;

public interface ITemplateRepository : IRepositoryWriter<Template>
{
    Task<Template?> GetActiveTemplateByCategoryAsync(string category,CancellationToken cancellationToken = default);

    Task<Template?> GetDefaultTemplateByCategoryAsync(string category, CancellationToken cancellationToken = default);
}
