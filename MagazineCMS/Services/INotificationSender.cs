namespace MagazineCMS.Services
{
    public interface INotificationSender
    {
        void SubmitContributionNotification(int facultyId, string userId, string magazineId, string contributionId);
    }
}
