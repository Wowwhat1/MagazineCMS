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

        //[HttpPost]
        //public IActionResult UpdateContribution(int contributionId)
        //{
        //    try
        //    {
        //        // Retrieve the contribution from the database
        //        var contribution = _unitOfWork.Contribution.Get(contributionId);

        //        // Check if the contribution exists
        //        if (contribution == null)
        //        {
        //            TempData["Error"] = "Contribution not found.";
        //            return RedirectToAction("Index");
        //        }

        //        // Perform the update operation (example: changing status from "Pending" to "Approved")
        //        contribution.Status = "Approved"; // Change this according to your update logic

        //        // Update the contribution in the database
        //        _unitOfWork.Contribution.Update(contribution);
        //        _unitOfWork.Save();

        //        TempData["Success"] = "Contribution updated successfully.";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"An error occurred while updating the contribution: {ex.Message}";
        //    }

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public async Task<IActionResult> SubmitContribution(ContributionSubmissionVM model, int magazineId)
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

                    // Save each file in the user's folder
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

                        var contribution = new Contribution
                        {
                            Title = fileName, // Set the title to the file name for now
                            Status = "Pending", // Set the status to pending
                            SubmissionDate = DateTime.Now,
                            UserId = userId,
                            MagazineId = magazineId 
                        };

                        // Add the contribution to the database context
                        _unitOfWork.Contribution.Add(contribution);

                        // Save changes to the database
                        _unitOfWork.Save();

                        // Create a new Document entity
                        var document = new Document
                        {
                            Type = "Uploaded",
                            DocumentUrl = fileName,
                            // Set ContributionId to the Id of the contribution being submitted
                            ContributionId = contribution.Id
                        };

                        // Add the document to the database context
                        _unitOfWork.Document.Add(document);

                        // Save changes to the database
                        _unitOfWork.Save();
                    }

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
