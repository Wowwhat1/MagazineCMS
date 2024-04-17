using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MagazineCMS.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    [Authorize(Roles = SD.Role_Coordinator)]

    public class MagazineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public MagazineController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            string userEmail = User.Identity.Name;
            var faculty = _unitOfWork.User.Get(u => u.Email == userEmail, includeProperties: "Faculty").Faculty;
            List<Magazine> magazineList = _unitOfWork.Magazine.GetAll(filter: x => x.FacultyId == faculty.Id, includeProperties: "Faculty,Semester").ToList();
            List<Magazine> closedMagazines = magazineList.Where(m => m.EndDate <= DateTime.Now).ToList();
            List<Magazine> openMagazines = magazineList.Where(m => m.EndDate > DateTime.Now).ToList();
            return View(new Tuple<List<Magazine>, List<Magazine>, string>(openMagazines, closedMagazines, faculty.Name));
        }

        public IActionResult Details(int id)
        {
            var magazine = _unitOfWork.Magazine.Get(m => m.Id == id, includeProperties: "Faculty,Semester");
            var contributions = _unitOfWork.Contribution.GetAll( c =>
                c.MagazineId == id,
                includeProperties: "Documents,User"
                ).ToList();
            return View(new Tuple<Magazine, List<Contribution>>(magazine, contributions));
        }

        public async Task<IActionResult> ContributionDetails(int id)
        {
            var contribution = _unitOfWork.Contribution.Get(x => x.Id == id, includeProperties: "User,Magazine,Documents,Feedbacks");
            var listDocument = _unitOfWork.Document.GetAll(d => d.ContributionId == contribution.Id
                                                    && d.Contribution.UserId == contribution.UserId
                                                    && d.Contribution.MagazineId == contribution.MagazineId);
            contribution.Documents = listDocument.ToList();
            contribution.Feedbacks = _unitOfWork.Feedback.GetAll(f => f.ContributionId == id).ToList();
            foreach (var feedback in contribution.Feedbacks)
            {
                var user = await _userManager.FindByIdAsync(feedback.UserId);
                if (user != null)
                {
                    feedback.User = new User { UserName = user.UserName };
                }
            }


            return View(contribution);
        }
        public IActionResult DownloadDocument(int documentId)
        {
            var document = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (document != null)
            {
                // Lấy đường dẫn tệp
                var filePath = document.DocumentUrl;

                // Kiểm tra xem tệp tồn tại trước khi tải xuống
                if (System.IO.File.Exists(filePath))
                {
                    // Trả về tệp để tải xuống
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    var fileName = Path.GetFileName(filePath);
                    return File(fileBytes, "application/octet-stream", fileName);
                }
            }
            // Nếu không tìm thấy tệp, trả về NotFound
            return NotFound();
        }
        public IActionResult ViewDocument(int documentId)
        {
            var document = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (document != null)
            {
                var fileBytes = System.IO.File.ReadAllBytes(document.DocumentUrl);
                var fileName = Path.GetFileName(document.DocumentUrl);
                var fileExtension = Path.GetExtension(document.DocumentUrl).ToLower();
                var fileType = GetMimeType(fileExtension); // Lấy kiểu MIME từ phần mở rộng của tệp

                return File(fileBytes, fileType);
            }
            return NotFound();
        }

        // Phương thức để lấy kiểu MIME từ phần mở rộng của tệp
        private string GetMimeType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".png":
                    return "image/png";
                case ".jpg":
                    return "image/jpeg";
                // Thêm các loại tệp khác nếu cần
                default:
                    return "application/octet-stream"; // Mặc định là kiểu MIME không xác định
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFeedback(FeedbackVM feedbackVM)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var feedback = new Feedback
                {
                    Comment = feedbackVM.Comment,
                    FeedbackDate = DateTime.Now,
                    ContributionId = feedbackVM.ContributionId,
                    UserId = userId,
                    Status = feedbackVM.Status // Lưu trạng thái của feedback từ trường Status trong FeedbackVM
                };

                _unitOfWork.Feedback.Add(feedback);

                // Nếu feedback được chọn là "Approved", cập nhật trạng thái của đóng góp thành "Approved"
                if (feedbackVM.Status == "Approved")
                {
                    var contribution = _unitOfWork.Contribution.Get(c => c.Id == feedback.ContributionId);
                    if (contribution != null)
                    {
                        contribution.Status = "Approved";
                        _unitOfWork.Contribution.Update(contribution);
                    }
                }
                else if (feedbackVM.Status == "Rejected")
                {
                    var contribution = _unitOfWork.Contribution.Get(c => c.Id == feedback.ContributionId);
                    if (contribution != null)
                    {
                        contribution.Status = "Rejected";
                        _unitOfWork.Contribution.Update(contribution);
                    }
                }

                _unitOfWork.Save();

                TempData["Success"] = "Feedback added successfully.";
            }
            else
            {
                TempData["Error"] = "Invalid model state. Please check your inputs.";
            }

            return RedirectToAction("Details", new { id = feedbackVM.ContributionId });
        }

        public IActionResult ViewFeedbacks(int contributionId)
        {
            var feedbacks = _unitOfWork.Feedback.GetAll(f => f.ContributionId == contributionId, includeProperties: "User");
            return View(feedbacks);
        }
    }
}
