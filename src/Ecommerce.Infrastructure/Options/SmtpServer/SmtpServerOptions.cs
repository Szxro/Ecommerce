using Ecommerce.Infrastructure.Common;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Infrastructure.Options.Smtp;

public class SmtpServerOptionsSetup : BaseOptions<SmtpServerOptions>
{
    public SmtpServerOptionsSetup(IConfiguration configuration) : base(configuration) { }
}

public class SmtpServerOptions : IConfigurationOptions
{
    public string SectionName => "SmtpServerOptions";

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool UseSsl { get; set; } = false;

    public string FromAddress { get; set; } = string.Empty;
}