using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Attributes;
using Ecommerce.Infrastructure.Options.Smtp;
using Ecommerce.SharedKernel.Common;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace Ecommerce.Infrastructure.Services;

[Inject(ServiceLifetime.Transient)]
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpServerOptions _smtpServerOptions;

    private static readonly string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    private static readonly Random rng = new Random(); // static instance to avoid re-seeding

    public EmailService(
        ILogger<EmailService> logger,
        IOptions<SmtpServerOptions> options)
    {
        _logger = logger;
        _smtpServerOptions = options.Value;
    }
    public string GenerateCode(int length = 10)
    {
        char[] buffer = new char[length];

        for (int i = 0; i < length; i++)
        {
            buffer[i] = Chars[rng.Next(Chars.Length)];
        }

        return new string(buffer);
    }

    public async Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        using SmtpClient client = new SmtpClient();

        MimeMessage message = CreateMimeMessage(emailMessage);

        // client events
        client.Authenticated += OnAuthenticated!;

        client.Connected += OnConnected!;

        client.Disconnected += OnDisconnected!;

        try
        {
            await client.ConnectAsync(_smtpServerOptions.Host, _smtpServerOptions.Port, cancellationToken: cancellationToken);

            await client.AuthenticateAsync(_smtpServerOptions.Username, _smtpServerOptions.Password, cancellationToken);

            string response = await client.SendAsync(message, cancellationToken);

            _logger.LogInformation(
                "Email sent to '{toAddress}' with subject '{subject}'",
                emailMessage.ToAddress,
                emailMessage.Subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An unexpected error happen while trying to send an email to {toAddress} with the error message: {message}",
                emailMessage.ToAddress,
                ex.Message);

            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    private MimeMessage CreateMimeMessage(EmailMessage emailMessage)
    {
        MimeMessage mimeMessage = new MimeMessage();

        mimeMessage.Subject = emailMessage.Subject;

        mimeMessage.From.Add(MailboxAddress.Parse(emailMessage.ToAddress));

        mimeMessage.To.Add(MailboxAddress.Parse(emailMessage.ToAddress));

        mimeMessage.Body = new TextPart("html") { Text = emailMessage.Body };

        return mimeMessage;
    }

    private void OnAuthenticated(object sender, AuthenticatedEventArgs args)
    {
        _logger.LogInformation(
            "SMTP Authentication: '{Message}'",
            args.Message);
    }

    private void OnConnected(object sender, ConnectedEventArgs args)
    {
        _logger.LogInformation(
            "SMTP Connected: Host '{Host}', Port '{Port}'",
            args.Host,
            args.Port);
    }

    private void OnDisconnected(object sender, DisconnectedEventArgs args)
    {
        _logger.LogInformation(
            "SMTP Disconnected: Host '{Host}', Port '{Port}', Requested '{Requested}'",
            args.Host,
            args.Port,
            args.IsRequested ? "true" : "false");
    }
}