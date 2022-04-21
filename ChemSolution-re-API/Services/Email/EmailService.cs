﻿using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace ChemSolution_re_API.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _serviceEmail = "fa3af6e8a3a1e8";
        private readonly string _serviceEmailPassword = "fabcf2955ab190";

        public async Task SendEmailAsync(string to, string subject, string html, string? from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? "ChemSolution-re-API"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.mailtrap.io", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_serviceEmail, _serviceEmailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
