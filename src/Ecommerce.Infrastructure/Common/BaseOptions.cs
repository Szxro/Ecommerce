using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.Common;

public abstract class BaseOptions<TOptions> : IConfigureOptions<TOptions>
    where TOptions : class, IConfigurationOptions
{
    private readonly IConfiguration _configuration;

    protected BaseOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(TOptions options)
    {
        _configuration
            .GetSection(options.SectionName)
            .Bind(options);
    }
}