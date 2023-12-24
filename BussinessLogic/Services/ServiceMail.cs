
using Microsoft.Extensions.Options;
using BussinessLogic.DTO.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BussinessLogic.Services
{
    public class ServiceMail
    {


        //instancio el settings para poder usar las credenciales de email
        private readonly EmailSettings _mailSettings;


        //inyecto el settings por el constructor, para poder usar las credenciales de email
        public ServiceMail(IOptions<EmailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }


        public async Task SendEmailAsync(string toAddress, string subject, string body, byte[] attachment = null, string attachmentName = "attachment.pdf")
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailSettings.SmtpUsername));
            email.To.Add(MailboxAddress.Parse(toAddress));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            if (attachment != null)
            {
                builder.Attachments.Add(attachmentName, attachment);
            }
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
            // Cambio realizado aquí para usar la contraseña de aplicación
            await smtp.AuthenticateAsync(_mailSettings.SmtpUsername, _mailSettings.SmtpPasswordFactores);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

    }
}