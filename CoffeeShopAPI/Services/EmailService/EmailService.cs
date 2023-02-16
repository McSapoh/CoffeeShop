using CoffeeShopAPI.Helpers.DTO.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public EmailService(IConfiguration config, ILogger<AuthService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendEmail(EmailDTO emailDTO)
        {
            // Getting sender info from configuration.
            var Using = _config["Email:Using"];
            var SenderEmail = _config[$"Email:{Using}:SenderEmail"];
            var SenderPassword = _config[$"Email:{Using}:SenderPassword"];
            var Host = _config[$"Email:{Using}:Host"];
            var Port = Convert.ToInt32(_config[$"Email:{Using}:Port"]);

            // Building Email.
            var Email = new MimeMessage();
            Email.From.Add(MailboxAddress.Parse(SenderEmail));
            Email.To.Add(MailboxAddress.Parse(emailDTO.ToEmail));
            Email.Subject = emailDTO.Subject;
            Email.Body = new TextPart { Text = emailDTO.Body };

            // Sending Email.
            try
            {
                var smtp = new SmtpClient();
                smtp.Connect(Host, Port);
                smtp.Authenticate(SenderEmail, SenderPassword);
                await smtp.SendAsync(Email);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
