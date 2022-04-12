using EipqLibrary.EmailService.Interfaces;
using EipqLibrary.EmailService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EipqLibrary.EmailService.Services
{
    internal class EmailService : IEmailService
    {
        private readonly IMessageDeliveryClientFactory<SmtpClient> _clientFactory;

        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IMessageDeliveryClientFactory<SmtpClient> clientFactory,
                            EmailSettings emailSettings,
                            ILogger<EmailService> logger)
        {
            _clientFactory = clientFactory;
            _emailSettings = emailSettings;
            _logger = logger;
        }

        public async Task SendEmailMessageAsync(MailMessage message)
        {
            try
            {
                message.Sender = SenderEmailAddress;
                message.From = SenderEmailAddress;

                var client = _clientFactory.Create(_emailSettings);
                await client.SendMailAsync(message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public MailMessage GenerateRegistrationDeniedMailMessage(string emailTo, string additionalMessage = null)
        {
            var mailMessage = GenerateMailMessage(emailTo, "ԵԻՊՔ Գրադարան - Ձեր հայտը մերժվել է");
            mailMessage.Body = $"Ձեր գրանցման հայտը մերժվել է ադմինիստրատորի կողմից!\n";
            mailMessage.Body += additionalMessage;
            mailMessage.IsBodyHtml = false;
            return mailMessage;
        }

        public MailMessage GenerateRegistrationConfirmedMailMessage(string emailTo)
        {
            var mailMessage = GenerateMailMessage(emailTo, "ԵԻՊՔ Գրադարան - Ձեր հայտը հաստատվել է");
            mailMessage.Body = $"Ձեր գրանցման հայտը հաստատվել է ադմինիստրատորի կողմից!\n" +
                                $"Դուք կարող եք մուտք գործել ձեր հաշիվ մուտքագրելով ձեր էլ․ հասցեն և գաղտնաբառը գրադարանի կայքի 'մուտք' էջում";
            mailMessage.IsBodyHtml = false;
            return mailMessage;
        }

        public MailMessage GenerateAdminRegistrationMailMessage(string emailTo, string userPassword)
        {
            var mailMessage = GenerateMailMessage(emailTo, "ԵԻՊՔ Գրադարան - Ձեր նոր հաշվի տվյալները");
            mailMessage.Body = $"Բարի գալուստ ԵԻՊՔ Գրադարան! <br> Օգտագործեք հետևյալ գաղտնաբառը ձեր հաշիվ մուտք գործելու համար: <b>{userPassword}</b>";
            mailMessage.IsBodyHtml = true;
            return mailMessage;
        }

        public MailMessage GenerateResetPasswordMailMessage(string emailTo, string token)
        {
            var mailMessage = GenerateMailMessage(emailTo, "ԵԻՊՔ Գրադարան - Փոխել գաղտնաբառը");
            mailMessage.Body = $"Գաղտնաբառը փոխելու համար անցեք հետևյալ հղումով\n" +
                $"http://notreadyyet.com/idk/{token}";

            //TODO: mi ban en chi
            mailMessage.IsBodyHtml = false;
            return mailMessage;
        }

        // Private methods
        private MailMessage GenerateMailMessage(string emailTo, string subject)
        {
            var mailMessage = new MailMessage { Sender = SenderEmailAddress, From = SenderEmailAddress };
            mailMessage.To.Add(emailTo);
            mailMessage.Subject = subject;
            return mailMessage;
        }

        public MailMessage GenerateAccountWasDeletedMailMessage(string emailTo, string additionalMessage = null)
        {
            var mailMessage = GenerateMailMessage(emailTo, "ԵԻՊՔ Գրադարան - Ձեր հաշիվը ջնջվել է");
            mailMessage.Body = $"Ձեր հաշիվը քոլեջի գրադարանի կայքում ջնջվել է!\n";
            mailMessage.Body += additionalMessage;
            mailMessage.IsBodyHtml = false;
            return mailMessage;
        }

        private MailAddress SenderEmailAddress => new MailAddress(_emailSettings.Mail, _emailSettings.DisplayName);
    }
}
