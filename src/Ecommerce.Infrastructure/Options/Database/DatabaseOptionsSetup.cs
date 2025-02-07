using Ecommerce.Infrastructure.Common;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Infrastructure.Options.Database;

public sealed class DatabaseOptionsSetup : BaseOptions<DatabaseOptions>
{
    public DatabaseOptionsSetup(IConfiguration configuration) : base(configuration) { }
}
