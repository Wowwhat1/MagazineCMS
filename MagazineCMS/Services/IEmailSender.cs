namespace MagazineCMS.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string subject, string message, IEnumerable<string> coordinatorEmails);
    }
}
