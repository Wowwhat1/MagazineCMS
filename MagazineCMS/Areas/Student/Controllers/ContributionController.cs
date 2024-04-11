using Microsoft.AspNetCore.Mvc;
using MagazineCMS.DataAccess.Repository;
using MagazineCMS.Models;
using MagazineCMS.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace MagazineCMS.Areas.Student.Controllers
{
    [Area("Student")]
    public class ContributionController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public ContributionController(IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
        }

        public IActionResult ContributionDetails(int id)
        {
            var contribution = _unitOfWork.Contribution.Get(filter: c => c.Id == id, includeProperties: "Documents");
            if (contribution == null)
            {
                return NotFound(); // Return a 404 Not Found error if the contribution is not found
            }

            // Retrieve feedback for the contribution
            var feedback = _unitOfWork.Feedback.Get(filter: f => f.ContributionId == id);

            // Retrieve the magazine associated with the contribution
            var magazine = _unitOfWork.Magazine.Get(m => m.Id == contribution.MagazineId);

            // Retrieve the semester associated with the contribution
            var semester = _unitOfWork.Semester.Get(s => s.Id == magazine.SemesterId);

            // Return a tuple containing the contribution, feedback, and semester end date
            var model = (contribution, feedback, semester.EndDate);

            return View(model); // Pass the tuple to the ContributionDetails view
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile files, int contributionId)
        {
            try
            {
                if (files == null || files.Length == 0)
                {
                    return BadRequest("File is empty");
                }

                var contribution = _unitOfWork.Contribution.Get(filter: c => c.Id == contributionId);

                if (contribution == null)
                {
                    return NotFound("Contribution not found");
                }


                // Get the current user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Create a folder for the contribution if it doesn't exist
                var contributionFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", userId);
                if (!Directory.Exists(contributionFolderPath))
                {
                    Directory.CreateDirectory(contributionFolderPath);
                }


                // Save the file to the server
                var fileName = Path.GetFileName(files.FileName);
                var filePath = Path.Combine(contributionFolderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await files.CopyToAsync(stream);
                }

                // Create a document entry in the database
                var document = new Document
                {
                    Type = "Uploaded",
                    DocumentUrl = fileName,
                    ContributionId = contributionId
                };

                // Add the document to the database context
                _unitOfWork.Document.Add(document);

                // Update the submission date of the contribution
                contribution.SubmissionDate = DateTime.Now;

                _unitOfWork.Save();

                TempData["Success"] = "File added successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("ContributionDetails", new { id = contributionId });
        }


        [HttpPost]
        public IActionResult DeleteFile(int fileId, int contributionId)
        {
            try
            {
                // Get the document to be deleted
                var document = _unitOfWork.Document.GetFirstOrDefault(d => d.Id == fileId);

                if (document == null)
                {
                    return NotFound("Document not found");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Remove the file from the server
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", userId, document.DocumentUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Remove the document from the database
                _unitOfWork.Document.Remove(document);

                // Update the submission date of the contribution
                var contribution = _unitOfWork.Contribution.Get(c => c.Id == contributionId);
                if (contribution != null)
                {
                    contribution.SubmissionDate = DateTime.Now;
                }

                _unitOfWork.Save();

                TempData["Success"] = "File deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("ContributionDetails", new { id = contributionId });
        }
    }
}
