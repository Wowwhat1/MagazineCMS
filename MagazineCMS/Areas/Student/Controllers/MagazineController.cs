using MagazineCMS.DataAccess.Repository;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
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

        public MagazineController(IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
            if (ModelState.IsValid)
            {
                try
                {
                    // Get the current user's ID
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    // Create a contribution
                    var contribution = new Contribution
                    {
                        Title = model.Files.FirstOrDefault()?.FileName ?? "Untitled",
                        Status = "Pending", // Set the status to pending
                        SubmissionDate = DateTime.Now,
                        UserId = userId,
                        MagazineId = magazineId
                    };

                    // Add the contribution to the database context
                    _unitOfWork.Contribution.Add(contribution);
                    _unitOfWork.Save();

                    // Create a folder for the contribution if it doesn't exist
                    var contributionFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", userId);
                    if (!Directory.Exists(contributionFolderPath))
                    {
                        Directory.CreateDirectory(contributionFolderPath);
                    }

                    // Loop through each file and save it
                    foreach (var file in model.Files)
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

                    TempData["Success"] = "Contribution submitted successfully.";
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

        #endregion

    }
}
