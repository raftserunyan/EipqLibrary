using System.Net.Mail;
using System.Threading.Tasks;

namespace EipqLibrary.EmailService.Interfaces
{
    public interface IEmailService
    {
        public MailMessage GenerateRegistrationDeniedMailMessage(string emailTo, string additionalMessage = null);
        public MailMessage GenerateRegistrationConfirmedMailMessage(string emailTo);
        public Task SendEmailMessageAsync(MailMessage message);
    }
}
