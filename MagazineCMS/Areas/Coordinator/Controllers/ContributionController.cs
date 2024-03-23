using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using System.Security.Claims;


namespace MagazineCMS.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    [Authorize(Roles = SD.Role_Coordinator)]
    public class ContributionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ContributionController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            var contributions = _unitOfWork.Contribution.GetAll(includeProperties: "User,Magazine").ToList();
            return View(contributions);
        }
        public IActionResult Details(int id)
        {
            var contribution = _unitOfWork.Contribution.Get(x => x.Id == id, includeProperties: "User,Magazine,Documents,Feedbacks");
            var listDocument = _unitOfWork.Document.GetAll(d => d.ContributionId == contribution.Id
                                                    && d.Contribution.UserId == contribution.UserId
                                                    && d.Contribution.MagazineId == contribution.MagazineId);
            contribution.Documents = listDocument.ToList();
            contribution.Feedbacks = _unitOfWork.Feedback.GetAll(f => f.ContributionId == id).ToList();
            foreach (var feedback in contribution.Feedbacks)
            {
                feedback.User = _unitOfWork.User.Get(u => u.Id == feedback.UserId);
            }
            return View(contribution);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string status, int contributionId)
        {
            var contribution = await _unitOfWork.Contribution.GetFirstOrDefaultAsync(c => c.Id == contributionId);
            if (contribution == null)
            {
                return NotFound();
            }

            contribution.Status = status;
            _unitOfWork.Save();

            return RedirectToAction("Details", new { id = contributionId });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitDocuments(ContributionSubmissionVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get the current user's ID
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    // Create a folder for the user if it doesn't exist
                    var userFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", userId);
                    if (!Directory.Exists(userFolderPath))
                    {
                        Directory.CreateDirectory(userFolderPath);
                    }

                    // Create a list to store documents
                    var documents = new List<Document>();

                    // Save each file in the user's folder and database
                    foreach (var file in model.Files)
                    {
                        // Generate a unique file name
                        var fileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";

                        // Combine the user's folder path with the file name
                        var filePath = Path.Combine(userFolderPath, fileName);

                        // Copy the file to the destination path
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Create a new document object
                        var document = new Document
                        {
                            Type = "Type of Document", // Set the type of document
                            DocumentUrl = filePath // Set the file path
                        };

                        // Add the document to the list
                        documents.Add(document);
                    }

                    // Save documents to the database
                    foreach (var document in documents)
                    {
                        // Assign the contribution ID
                        document.ContributionId = model.ContributionId;

                        // Add the document to the database
                        _unitOfWork.Document.Add(document);
                    }

                    // Save changes to the database
                    await _unitOfWork.SaveAsync();

                    TempData["Success"] = "Documents uploaded successfully.";
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
                    UserId = userId
                };

                _unitOfWork.Feedback.Add(feedback);
                await _unitOfWork.SaveAsync();

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
