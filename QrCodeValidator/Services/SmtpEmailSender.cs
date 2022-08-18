using System.IO;
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

        public void SendEmail(string fromAddress, string toAddress, string subject, string path)
        {
     

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(fromAddress ?? _options.Value.UserName));
            email.To.Add(MailboxAddress.Parse(toAddress));
            email.Subject = subject;
            //email.Body = new TextPart(TextFormat.Html) { Text = message };

            // create our message text, just like before (except don't set it as the message.Body)
            var body = new TextPart("plain")
            {
                Text = @"Hey Alice,

                    What are you up to this weekend? Monica is throwing one of her parties on
                    Saturday. I was hoping you could make it.

                    Will you be my +1?

                    -- Joey
                    "
            };
            // create an image attachment for the file located at path
            var attachment = new MimePart("image", "gif")
            {
                Content = new MimeContent(File.OpenRead(path)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(path)
            };

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            // now set the multipart/mixed as the message body
            email.Body = multipart;

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.UserName, _options.Value.Password);
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}

