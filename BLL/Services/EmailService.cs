using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace BLL.Services
{
    /// <summary>
    /// Performs email operations such as sending email.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public async Task Send(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.Add(new MailboxAddress(_emailConfiguration.SenderName, _emailConfiguration.SmtpUsername));
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart(TextFormat.Plain) {Text = emailMessage.Content};

            using var emailClient = new SmtpClient();
            await emailClient.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
            await emailClient.AuthenticateAsync(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
            await emailClient.SendAsync(message);
            await emailClient.DisconnectAsync(true);
        }
    }
}