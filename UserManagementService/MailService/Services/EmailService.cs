using MailKit.Net.Smtp;
using MimeKit;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.MailService.Models;

namespace UserManagementService.MailService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _configuration;
        public EmailService(EmailConfiguration emailConfiguration)
        {
            _configuration = emailConfiguration;
            
        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _configuration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }
        private void Send(MimeMessage emailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_configuration.SmtpServer, _configuration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_configuration.UserName, _configuration.Password);
                client.Send(emailMessage);

            }catch 
            {
                throw;

            }finally
            {
                client.Disconnect(true);
                client.Dispose();
            }

        }
    }
}
