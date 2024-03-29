using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using MDriven.MDrivenServer;

namespace MagazineCMS.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "chienhuynh234@gmail.com";
            var password = "qWemNb123098!!??";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(
                new MailMessage(from: mail,
                                       to: email,
                                       subject,
                                       message
                                       ));
        }
    }
}
