using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace IntelART.Communication
{
    public class SmtpEmailSender : IEmailSender
    {
        private string host;
        private int port;
        private string login;
        private string password;
        private bool useSsl;
        private string defaultFromAddress;

        public SmtpEmailSender(string host, int port, string login, string password, string defaultFromAddress, bool useSsl)
        {
            this.host = host;
            this.port = port;
            this.login = login;
            this.password = password;
            this.useSsl = useSsl;
            this.defaultFromAddress = defaultFromAddress;
        }

        private MailboxAddress GetMailboxAddress(EmailAddress address)
        {
            MailboxAddress result = new MailboxAddress(address.FullName, address.Address);

            return result;
        }

        private MailboxAddress GetMailboxAddress(string address)
        {
            return new MailboxAddress(address);
        }

        private async Task SendInternalAsync(MailboxAddress from, MailboxAddress to, string subject, string body)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(from);
            message.To.Add(to);
            message.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            using (SmtpClient client = new SmtpClient())
            {
                await client.ConnectAsync(this.host, this.port, this.useSsl);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(this.login, this.password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendAsync(EmailAddress from, EmailAddress to, string subject, string body)
        {
            await this.SendInternalAsync(this.GetMailboxAddress(from), this.GetMailboxAddress(to), subject, body);
        }

        public async Task SendAsync(EmailAddress to, string subject, string body)
        {
            await this.SendInternalAsync(this.GetMailboxAddress(this.defaultFromAddress), this.GetMailboxAddress(to), subject, body);
        }
    }
}
