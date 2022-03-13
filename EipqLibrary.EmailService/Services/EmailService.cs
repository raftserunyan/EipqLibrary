using EipqLibrary.EmailService.Interfaces;
using EipqLibrary.EmailService.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EipqLibrary.EmailService.Services
{
    internal class EmailService : IEmailService
    {
        private readonly IMessageDeliveryClientFactory<SmtpClient> _clientFactory;

        private readonly EmailSettings _emailSettings;

        public EmailService(IMessageDeliveryClientFactory<SmtpClient> clientFactory,
                            EmailSettings emailSettings)
        {
            _clientFactory = clientFactory;
            _emailSettings = emailSettings;
        }

        public async Task SendEmailMessageAsync(MailMessage message)
        {
            message.Sender = SenderEmailAddress;
            message.From = SenderEmailAddress;

            var client = _clientFactory.Create(_emailSettings);
            await client.SendMailAsync(message);
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

        private MailMessage GenerateMailMessage(string emailTo, string subject)
        {
            var mailMessage = new MailMessage { Sender = SenderEmailAddress, From = SenderEmailAddress };
            mailMessage.To.Add(emailTo);
            mailMessage.Subject = subject;
            return mailMessage;
        }

        private MailAddress SenderEmailAddress => new MailAddress(_emailSettings.Mail, _emailSettings.DisplayName);
    }
}
