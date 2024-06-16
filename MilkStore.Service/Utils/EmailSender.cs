using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace MilkStore.Service.Utils
{
    public class EmailSender : IEmailSender
    {
        private readonly AppConfiguration _emailSettings;

        public EmailSender(AppConfiguration emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.Email.SenderName, _emailSettings.Email.SenderEmail));
            email.To.Add(new MailboxAddress(toEmail, toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(_emailSettings.Email.SmtpServer, _emailSettings.Email.SmtpPort, _emailSettings.Email.UseSSL);
                await smtp.AuthenticateAsync(_emailSettings.Email.Username, _emailSettings.Email.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
