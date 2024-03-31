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
    }
}
