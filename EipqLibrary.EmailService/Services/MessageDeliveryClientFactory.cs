using System;
using System.Net;
using System.Net.Mail;
using EipqLibrary.EmailService.Interfaces;
using EipqLibrary.EmailService.Models;

namespace EipqLibrary.EmailService.Services
{
    internal class MessageDeliveryClientFactory<TClient> : IMessageDeliveryClientFactory<TClient>
        where TClient : SmtpClient
    {
        public TClient Create(EmailSettings emailSettings)
        {
            if (typeof(TClient) == typeof(SmtpClient))
            {
                return (TClient)new SmtpClient(emailSettings.Host, emailSettings.Port)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailSettings.Mail, emailSettings.Password)
                };
            }

            throw new InvalidOperationException();
        }
    }
}
