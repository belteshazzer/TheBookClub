using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace TheBookClub.Common.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));

            if (string.IsNullOrWhiteSpace(_emailSettings.SmtpUser))
            {
                throw new ArgumentException("SMTP user (email address) must be provided in the configuration.");
            }

            if (string.IsNullOrWhiteSpace(_emailSettings.SmtpHost))
            {
                throw new ArgumentException("SMTP host must be provided in the configuration.");
            }

            if (_emailSettings.SmtpPort <= 0)
            {
                throw new ArgumentException("SMTP port must be a positive number.");
            }
        }
        public async Task<bool> SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort);
                smtpClient.Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                smtpClient.EnableSsl = _emailSettings.EnableSsl;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SmtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                await smtpClient.SendMailAsync(mailMessage);

                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}