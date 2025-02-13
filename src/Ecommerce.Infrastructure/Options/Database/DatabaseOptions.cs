using Ecommerce.Infrastructure.Common;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Infrastructure.Options.Database;

public sealed class DatabaseOptionsSetup : BaseOptions<DatabaseOptions>
{
    public DatabaseOptionsSetup(IConfiguration configuration) : base(configuration) { }
}

public sealed class DatabaseOptions : IConfigurationOptions
{
    public string SectionName => "DatabaseOptions";

    public string ConnectionString { get; set; } = string.Empty;

    public int CommandTimeout { get; set; }

    public bool EnableDetailedErrors { get; set; }

    public bool EnableSensitiveDataLogging { get; set; }
}
