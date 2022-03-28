using System.Threading.Tasks;

namespace IntelART.Communication
{
    public interface IEmailSender
    {
        Task SendAsync(EmailAddress from, EmailAddress to, string subject, string body);
        Task SendAsync(EmailAddress to, string subject, string body);
    }
}
