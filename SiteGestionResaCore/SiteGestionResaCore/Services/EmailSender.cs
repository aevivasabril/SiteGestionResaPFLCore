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
            var message = new MimeMessage();
            
                message.From.Add(MailboxAddress.Parse(options.Value.From));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            if (body.IndexOf("<html", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }
            //if (attachments?.Any() == true)
            //{
            //    foreach (var (data, name) in attachments)
            //    {
            //        bodyBuilder.Attachments.Add(name, data);
            //    }
            //}
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(options.Value.SMTP, 587, true).ConfigureAwait(false);
                await client.AuthenticateAsync(options.Value.UserName, options.Value.Password).ConfigureAwait(false);
                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
