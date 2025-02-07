using Ecommerce.SharedKernel.Contracts;

namespace Ecommerce.Infrastructure.Options.Database;

public sealed class DatabaseOptions : IConfigurationOptions
{
    public string SectionName => "DatabaseOptions";

    public string ConnectionString { get; set; } = string.Empty;

    public int CommandTimeout { get; set; }

    public bool EnableDetailedErrors { get; set; }

    public bool EnableSensitiveDataLogging { get; set; }
}
