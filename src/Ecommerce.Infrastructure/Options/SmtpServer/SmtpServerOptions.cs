using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.Options.Smtp;

public class SmtpServerOptionsSetup : IPostConfigureOptions<SmtpServerOptions>
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IConfiguration _configuration;

    public SmtpServerOptionsSetup(
        IHostEnvironment hostEnvironment,
        IConfiguration configuration)
    {
        _hostEnvironment = hostEnvironment;
        _configuration = configuration;
    }
    public void PostConfigure(string? name, SmtpServerOptions options)
    {
        string sectionName = _hostEnvironment.IsDevelopment() 
            ? "SmtpServerDevOptions"
            : options.SectionName;

        _configuration
            .GetSection(sectionName)
            .Bind(options);
    }
}

public class SmtpServerOptions : IConfigurationOptions
{
    // In this case it represent the production options
    public string SectionName => "SmtpServerOptions";

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool UseSsl { get; set; } = false;

    public string FromAddress { get; set; } = string.Empty;
}