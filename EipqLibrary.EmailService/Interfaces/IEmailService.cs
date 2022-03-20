using System.Net.Mail;
using System.Threading.Tasks;

namespace EipqLibrary.EmailService.Interfaces
{
    public interface IEmailService
    {
        MailMessage GenerateAdminRegistrationMailMessage(string emailTo, string userPassword);
        MailMessage GenerateRegistrationDeniedMailMessage(string emailTo, string additionalMessage = null);
        MailMessage GenerateRegistrationConfirmedMailMessage(string emailTo);
        MailMessage GenerateResetPasswordMailMessage(string emailTo, string token);
        Task SendEmailMessageAsync(MailMessage message);
    }
}
