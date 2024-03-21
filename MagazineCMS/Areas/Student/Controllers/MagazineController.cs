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
            var magazine = _unitOfWork.Magazine.Get(x => x.Id == id, includeProperties: "Faculty,Semester");
            return View(magazine);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContribution(ContributionSubmissionVM model)
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
