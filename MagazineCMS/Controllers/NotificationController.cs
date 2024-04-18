using MagazineCMS.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MagazineCMS.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            var notifications = _unitOfWork.Notification.GetAll(n => n.RecipientUserId == userId).OrderByDescending(n => n.CreatedAt).ToList() ;
            foreach (var notification in notifications)
            {
                var userIds = notification.UserIds;
                var count = userIds.Count;

                if (count == 1)
                {
                    var userName = GetUserName(userIds[0]);
                    // Handle notification with single user
                    notification.SenderUserName = userName;
                }
                else if (count == 2)
                {
                    var userName1 = GetUserName(userIds[0]);
                    var userName2 = GetUserName(userIds[1]);
                    // Handle notification with two users
                    notification.SenderUserName = $"{userName1} and {userName2}";
                }
                else if (count > 2)
                {
                    var userName1 = GetUserName(userIds[0]);
                    var userName2 = GetUserName(userIds[1]);
                    var remainingCount = count - 2;
                    var remainingUsers = string.Join(", ", userIds.Skip(2).Take(remainingCount));
                    notification.SenderUserName = $"{userName1}, {userName2} and {remainingCount} people";
                    // Handle notification with more than two users
                }
            }
            return View(notifications);
        }

        public string GetUserName(string userId)
        {
            var user = _unitOfWork.User.Get(u => u.Id == userId);
            return user.Firstname + " " + user.Lastname;
        }

        #region API CALL
        public IActionResult GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            var notifications = _unitOfWork.Notification.GetAll(n => n.RecipientUserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
            foreach (var notification in notifications)
            {
                var userIds = notification.UserIds;
                var count = userIds.Count;

                if (count == 1)
                {
                    var userName = GetUserName(userIds[0]);
                    // Handle notification with single user
                    notification.SenderUserName = userName;
                }
                else if (count == 2)
                {
                    var userName1 = GetUserName(userIds[0]);
                    var userName2 = GetUserName(userIds[1]);
                    // Handle notification with two users
                    notification.SenderUserName = $"{userName1} and {userName2}";
                }
                else if (count > 2)
                {
                    var userName1 = GetUserName(userIds[0]);
                    var userName2 = GetUserName(userIds[1]);
                    var remainingCount = count - 2;
                    var remainingUsers = string.Join(", ", userIds.Skip(2).Take(remainingCount));
                    notification.SenderUserName = $"{userName1}, {userName2} and {remainingCount} people";
                    // Handle notification with more than two users
                }
            }

            return Json(new { data = notifications });
        }

        public IActionResult GetFewNotification()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            var notifications = _unitOfWork.Notification.GetAll(n => n.RecipientUserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5) // Get maximum 5 notifications
                .ToList();
            foreach (var notification in notifications)
            {
                var userIds = notification.UserIds;
                var count = userIds.Count;

                if (count == 1)
                {
                    var userName = GetUserName(userIds[0]);
                    // Handle notification with single user
                    notification.SenderUserName = userName;
                }
                else if (count == 2)
                {
                    var userName1 = GetUserName(userIds[0]);
                    var userName2 = GetUserName(userIds[1]);
                    // Handle notification with two users
                    notification.SenderUserName = $"{userName1} and {userName2}";
                }
                else if (count > 2)
                {
                    var userName1 = GetUserName(userIds[0]);
                    var userName2 = GetUserName(userIds[1]);
                    var remainingCount = count - 2;
                    var remainingUsers = string.Join(", ", userIds.Skip(2).Take(remainingCount));
                    notification.SenderUserName = $"{userName1}, {userName2} and {remainingCount} people";
                    // Handle notification with more than two users
                }
            }

            return Json(new { data = notifications });
        }

        [HttpPost]
        public IActionResult MarkAsRead(int id)
        {
            var notification = _unitOfWork.Notification.Get(n => n.Id == id);
            if (notification == null)
            {
                return Json(new { success = false, message = "Error while marking notification as read" });
            }
            notification.IsRead = true;
            _unitOfWork.Notification.Update(notification);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Notification marked as read" });
        }

        #endregion
    }
}
