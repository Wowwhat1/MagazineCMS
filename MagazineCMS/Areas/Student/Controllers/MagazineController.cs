using MagazineCMS.DataAccess.Repository;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using MagazineCMS.Services;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System.Data;
using System.Security.Claims;

namespace MagazineCMS.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = SD.Role_Student)]
    public class MagazineController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IUserRepository userRepository;

        public MagazineController(IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            string userEmail = User.Identity.Name;
            int userFaculty = _unitOfWork.User.Get(x => x.Email == userEmail).FacultyId;
            string facultyName = _unitOfWork.Faculty.Get(x => x.Id == userFaculty).Name;
            List<Magazine> magazineList = _unitOfWork.Magazine.GetAll(filter: x => x.FacultyId == 2, includeProperties: "Faculty,Semester").ToList();
            List<Magazine> closedMagazines = magazineList.Where(m => m.EndDate <= DateTime.Now).ToList();
            List<Magazine> openMagazines = magazineList.Where(m => m.EndDate > DateTime.Now).ToList();

            return View(new Tuple<List<Magazine>, List<Magazine>, string>(openMagazines, closedMagazines, facultyName)) ;
        }

        public IActionResult Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var magazine = _unitOfWork.Magazine.Get(x => x.Id == id, includeProperties: "Faculty,Semester");
            // Fetch conmodel: tribution history for the current user and selected magazine
            var contributions = _unitOfWork.Contribution.GetAll(
                filter: c => c.UserId == userId && c.MagazineId == id,
                includeProperties: "Documents");

            // Pass both magazine and contributions to the view
            var tuple = new Tuple<Magazine, IEnumerable<Contribution>>(magazine, contributions);
            return View(tuple);
        }

        [HttpGet]
        public IActionResult Download(int documentId)
        {
            var document = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (document != null)
            {
                var fileName = document.DocumentUrl; // Assume DocumentUrl contains only the file name, not the full path

                // Get the current user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", userId, fileName);

                // Check if the file exists
                if (System.IO.File.Exists(filePath))
                {
                    // Return the file for download
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, "application/octet-stream", fileName);
                }
            }
            // If the file is not found, return NotFound
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContribution(ContributionSubmissionVM model, int magazineId)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid model state. Please check your inputs.";
                return RedirectToAction("Index");
            }

            try
            {
                var userId = GetCurrentUserId();
                var userEmail = GetCurrentUserEmail();
                var contribution = CreateContribution(model, magazineId, userId);
                SaveContributionAndDocuments(model.Files, contribution);
                var magazineTitle = GetMagazineTitle(magazineId);
                var coordinatorEmails = await GetCoordinatorEmailsAsync();
                await SendContributionEmailToCoordinatorsAsync(userEmail, magazineTitle, coordinatorEmails);
                TempData["Success"] = "Contribution submitted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private string GetCurrentUserEmail()
        {
            return User.Identity.Name;
        }

        private Contribution CreateContribution(ContributionSubmissionVM model, int magazineId, string userId)
        {
            var contribution = new Contribution
            {
                Title = model.Files.FirstOrDefault()?.FileName ?? "Untitled",
                Status = "Pending",
                SubmissionDate = DateTime.Now,
                UserId = userId,
                MagazineId = magazineId
            };

            _unitOfWork.Contribution.Add(contribution);
            _unitOfWork.Save();

            return contribution;
        }

        private void SaveContributionAndDocuments(List<IFormFile> files, Contribution contribution)
        {
            var userId = contribution.UserId;
            var contributionFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", userId);

            if (!Directory.Exists(contributionFolderPath))
            {
                Directory.CreateDirectory(contributionFolderPath);
            }

            foreach (var file in files)
            {
                var document = new Document
                {
                    Type = "Uploaded",
                    DocumentUrl = file.FileName,
                    ContributionId = contribution.Id
                };

                _unitOfWork.Document.Add(document);
                _unitOfWork.Save();

                var filePath = Path.Combine(contributionFolderPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }

        private string GetMagazineTitle(int magazineId)
        {
            var magazine = _unitOfWork.Magazine.Get(u => u.Id == magazineId);
            return magazine.Name;
        }

        private async Task<IEnumerable<string>> GetCoordinatorEmailsAsync()
        {
            var userEmail = GetCurrentUserEmail();
            var user = _unitOfWork.User.Get(u => u.Email == userEmail);

            var coordinatorUsers = _unitOfWork.User.GetUserByFacultyIdAndRole(user.FacultyId, SD.Role_Coordinator);
            return coordinatorUsers.Select(u => u.Email);
        }

        private async Task SendContributionEmailToCoordinatorsAsync(string userEmail, string magazineTitle, IEnumerable<string> coordinatorEmails)
        {
            var subject = "IMPORTANT! CONTRIBUTION SUBMITTED";
            var message = $"{userEmail} has just submitted a contribution to {magazineTitle}. Please check and give feedback in 14 days!";
            await _emailSender.SendEmailAsync(subject, message, coordinatorEmails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContribution(int contributionId, IFormFile[] Files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing contribution
                    var contribution = _unitOfWork.Contribution.GetById(contributionId);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (contribution == null)
                    {
                        TempData["Error"] = "Contribution not found.";
                        return RedirectToAction("Index");
                    }

                    var contributionFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", userId);

                    // Delete existing documents associated with the contribution
                    var existingDocuments = _unitOfWork.Document.GetAll(d => d.ContributionId == contributionId);
                    foreach (var existingDocument in existingDocuments)
                    {
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", existingDocument.DocumentUrl);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        // Delete the file in the wwwroot/documents/StudentId folder
                        var studentFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", userId, existingDocument.DocumentUrl);
                        if (System.IO.File.Exists(studentFilePath))
                        {
                            System.IO.File.Delete(studentFilePath);
                        }

                        _unitOfWork.Document.Remove(existingDocument);
                    }

                    // Save changes to remove existing documents
                    _unitOfWork.Save();

                    // Save the new documents
                    foreach (var file in Files)
                    {
                        var document = new Document
                        {
                            Type = "Uploaded",
                            DocumentUrl = file.FileName,
                            ContributionId = contribution.Id
                        };

                        // Add the document to the database context
                        _unitOfWork.Document.Add(document);

                        // Save changes to the database
                        _unitOfWork.Save();

                        // Save the file to the server
                        var filePath = Path.Combine(contributionFolderPath, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                    TempData["Success"] = "Contribution updated successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Invalid model state. Please check your inputs.";
            }

            return RedirectToAction("Index");
        }

        public void SubmitContributionNotificationAsync(int facultyId, string userId)
        {

            var coordinators = _unitOfWork.User.GetUserByFacultyIdAndRole(facultyId, SD.Role_Coordinator);

            foreach (var coordinator in coordinators)
            {
                var oldNotification = _unitOfWork.Notification.GetAll()
                        .OrderByDescending(n => n.CreatedAt)
                        .FirstOrDefault(n => n.RecipientUserId == coordinator.Id && n.Type == SD.Noti_Type_SubmitSingle);

                if (oldNotification != null)
                {
                    oldNotification.UserIds.Add(userId);
                    oldNotification.CreatedAt = DateTime.Now;
                    _unitOfWork.Notification.Update(oldNotification);
                }
                else
                {
                    var notification = new Notification
                    {
                        RecipientUserId = coordinator.Id,
                        UserIds = new List<string> { userId },
                        Content = "submits a new contribution",
                        Type = SD.Noti_Type_SubmitSingle,
                        Url = "/#",
                        CreatedAt = DateTime.Now,
                        IsRead = false,
                    };

                    _unitOfWork.Notification.Add(notification);

                }
            }
            _unitOfWork.Save();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            string userEmail = User.Identity.Name;
            int userFaculty = _unitOfWork.User.Get(x => x.Email == userEmail).FacultyId;
            
            List<Magazine> magazineList = _unitOfWork.Magazine.GetAll(filter: x => x.FacultyId == 2,includeProperties: "Faculty,Semester" ).ToList();
            List<Magazine> closedMagazines = magazineList.Where(m => m.EndDate <= DateTime.Now).ToList();
            List<Magazine> openMagazines = magazineList.Where(m => m.EndDate > DateTime.Now).ToList();
            
            return Json(new { closedMagazines, openMagazines });
        }

        [HttpGet]
        public IActionResult GetNotification(string? userId)
        {
            var notifications = _unitOfWork.Notification.GetAll();
            return Json( new { data = notifications });
        }

        #endregion

    }
}
