using LearnAPI.Helper;
using LearnAPI.Service;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


namespace LearnAPI.Container
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            this.emailSettings = options.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            //attechment file from project directory
            byte[] fileBytes;
            if (System.IO.File.Exists("AttachmentsFiles/dummy.pdf"))
            {
                FileStream file = new FileStream("AttachmentsFiles/dummy.pdf",FileMode.Open, FileAccess.Read);
                using (var sr = new MemoryStream())
                {
                    file.CopyTo(sr);
                    fileBytes=sr.ToArray();
                }
                builder.Attachments.Add("AttachmentsFiles.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            //end



            using var smtp = new SmtpClient();

            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }
    }
}
