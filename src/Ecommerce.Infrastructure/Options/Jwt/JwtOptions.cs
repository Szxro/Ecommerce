using Ecommerce.Infrastructure.Common;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Infrastructure.Options.Jwt;

public class JwtOptionsSetup : BaseOptions<JwtOptions>
{
    public JwtOptionsSetup(IConfiguration configuration) : base(configuration) { }
}

public class JwtOptions : IConfigurationOptions
{
    public string SectionName => "JwtOptions";

    public bool ValidateAudience { get; set; }

    public bool ValidateIssuer { get; set; }

    public bool ValidateLifetime { get; set; }

    public string ValidIssuer { get; set; } = string.Empty;

    public string ValidAudience { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;
}