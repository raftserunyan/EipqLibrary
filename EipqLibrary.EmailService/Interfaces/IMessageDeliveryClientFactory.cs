using System.Net.Mail;
using EipqLibrary.EmailService.Models;

namespace EipqLibrary.EmailService.Interfaces
{
    public interface IMessageDeliveryClientFactory<out TClient> where TClient : SmtpClient
    {
        public TClient Create(EmailSettings settings);
    }
}
