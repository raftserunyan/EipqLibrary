using System.Net.Mail;
using System.Threading.Tasks;

namespace EipqLibrary.EmailService.Interfaces
{
    public interface IEmailService
    {
        public MailMessage GenerateRegistrationDeniedMailMessage(string emailTo, string additionalMessage = null);
        public Task SendEmailMessageAsync(MailMessage message);
    }
}
