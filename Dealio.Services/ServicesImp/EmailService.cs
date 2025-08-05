using Dealio.Domain.Helpers;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Dealio.Services.ServicesImp
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }

        public async Task<ServiceResultEnum> SendEmailAsync(EmailModel emailModel)
        {
            try
            {
                using var client = new SmtpClient();

                //connect with server
                await client.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);

                // authenticate
                await client.AuthenticateAsync(emailSettings.FromEmail, "agukurbgkjffrgns");

                var bodyBuilder = new BodyBuilder
                {
                    TextBody = emailModel.Message
                };

                var message = new MimeMessage
                {
                    Subject = emailModel.Subject ?? "No Subject",
                    Body = bodyBuilder.ToMessageBody()
                };

                message.From.Add(new MailboxAddress("Dealio", emailSettings.FromEmail));
                message.To.Add(new MailboxAddress("user", emailModel.Email));
                message.Subject = emailModel.Subject == null ? "No Submitted" : emailModel.Subject;
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return ServiceResultEnum.Success;
            }
            catch (Exception ex)
            {
                return ServiceResultEnum.Failed;
            }
        }
    }
}
