using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using MDriven.MDrivenServer;

namespace MagazineCMS.Services
{
    public class EmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        public Task SendEmailAsync(string subject, string message, IEnumerable<string> coordinatorEmails)
        {
            var mail = "chienhcgcd210012@fpt.edu.vn";
            var password = "frac mgwm udmc lhqc";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message
            };

            foreach (var coordinatorEmail in coordinatorEmails)
            {
                mailMessage.To.Add(coordinatorEmail);
            }

            return client.SendMailAsync(mailMessage);
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
