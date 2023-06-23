using System.Net.Mail;
using System.Net;

namespace JobSchedulingApi.Services.EmailServices
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken)
        {
            string mail = _configuration.GetValue<string>("EmailingCredentials:SenderEmail");
            string password = _configuration.GetValue<string>("EmailingCredentials:SenderPassword");

            if (String.IsNullOrEmpty(mail) || String.IsNullOrEmpty(password))
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail,
                                                        to: email,
                                                        subject,
                                                        message));
        }
    }
}
