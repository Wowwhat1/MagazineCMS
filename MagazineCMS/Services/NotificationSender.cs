using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;

namespace MagazineCMS.Services
{
    public class NotificationSender : INotificationSender
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationSender(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void SubmitContributionNotification(int facultyId, string userId, string magazineId, string contributionId)
        {
            var coordinators = _unitOfWork.User.GetUserByFacultyIdAndRole(facultyId, SD.Role_Coordinator);

            foreach (var coordinator in coordinators)
            {
                var oldNotification = _unitOfWork.Notification.GetAll()
                        .OrderByDescending(n => n.CreatedAt)
                        .FirstOrDefault(n => n.RecipientUserId == coordinator.Id && n.Type == SD.Noti_Type_SubmitSingle);

                if (oldNotification != null && !oldNotification.UserIds.Contains(userId) && (DateTime.Now - oldNotification.CreatedAt).TotalHours < 1)
                {
                    oldNotification.UserIds.Add(userId);
                    oldNotification.Url = "/Coordinator/"+magazineId;
                    oldNotification.CreatedAt = DateTime.Now;
                    oldNotification.IsRead = false;
                    _unitOfWork.Notification.Update(oldNotification);
                }
                else
                {
                    var notification = new Notification
                    {
                        RecipientUserId = coordinator.Id,
                        UserIds = new List<string> { userId },
                        SenderUserName = "",
                        Content = "submits a new contribution",
                        Type = SD.Noti_Type_SubmitSingle,
                        Url = "/Coordinator/"+magazineId+""+contributionId,
                        CreatedAt = DateTime.Now,
                        IsRead = false,
                    };

                    _unitOfWork.Notification.Add(notification);

                }
            }
            _unitOfWork.Save();
        }

        public void ContributionStatusNotification(string recipientUserId,string userId, string magazineId, string contributionId)
        {
            var status = _unitOfWork.Contribution.Get(c => c.Id == Convert.ToInt32(contributionId)).Status;
            var notification = new Notification
            {
                RecipientUserId = recipientUserId,
                UserIds = new List<string> { userId },
                SenderUserName = "",
                Content = "has feedback on your contribution",
                Type = status,
                Url = "/Student/" + magazineId + "" + contributionId,
                CreatedAt = DateTime.Now,
                IsRead = false,
            };

            _unitOfWork.Notification.Add(notification);
            _unitOfWork.Save();
        }

        public void SendContributionReminders()
        {
            // Get contributions that are overdue for review
            var overdueContributions = _unitOfWork.Contribution.GetAll()
                .Where(c => c.Status == "Pending" && DateTime.Now - c.SubmissionDate > TimeSpan.FromDays(14));

            foreach (var contribution in overdueContributions)
            {
                // Send reminder notifications to relevant users
                // Example: Send reminder to coordinators
                var coordinators = _unitOfWork.User.GetUserByFacultyIdAndRole(contribution.User.FacultyId, SD.Role_Coordinator);

                foreach (var coordinator in coordinators)
                {
                    // Check if a reminder has already been sent within the last hour
                    var lastReminder = _unitOfWork.Notification.GetAll()
                        .OrderByDescending(n => n.CreatedAt)
                        .FirstOrDefault(n => n.RecipientUserId == coordinator.Id && n.Type == SD.Noti_Type_ContributionReminder);

                    if (lastReminder == null || (DateTime.Now - lastReminder.CreatedAt).TotalHours >= 1)
                    {
                        // Create and send reminder notification
                        var reminderNotification = new Notification
                        {
                            RecipientUserId = coordinator.Id,
                            Content = $"Contribution '{contribution.Title}' is overdue for review.",
                            Type = SD.Noti_Type_ContributionReminder,
                            Url = $"/Coordinator/{contribution.MagazineId}/{contribution.Id}",
                            CreatedAt = DateTime.Now,
                            IsRead = false
                        };

                        _unitOfWork.Notification.Add(reminderNotification);
                    }
                }
            }

            _unitOfWork.Save();
        }
    }
}
