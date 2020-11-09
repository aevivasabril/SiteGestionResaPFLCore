using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SiteGestionResaCore.Opts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<EmailOptions> options;

        public EmailSender(IOptions<EmailOptions> options)
        {
            this.options = options;
        }
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient(options.Value.SMTP);

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;

            // Creatte the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(options.Value.UserName, options.Value.Password);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new System.Net.Mail.MailMessage(options.Value.From, email);

            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;

            // Send:
            await client.SendMailAsync(mail);
        }
    }
}
