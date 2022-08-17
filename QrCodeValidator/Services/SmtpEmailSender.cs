using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;



namespace FinApp.Services
{
    public class SmtpEmailSender : IEmailSender
    {

        private readonly IOptions<SmtpOptions> _options;

        public SmtpEmailSender(IOptions<SmtpOptions> options)
        {
            _options = options;
        }

        public void SendEmail(string fromAddress, string toAddress, string subject, string message)
        {
     

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(fromAddress ?? _options.Value.UserName));
            email.To.Add(MailboxAddress.Parse(toAddress));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.UserName, _options.Value.Password);
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}

