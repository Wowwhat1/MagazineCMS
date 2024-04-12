using MagazineCMS.DataAccess.Repository;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Magazine> openMagazines;
            List<Magazine> closedMagazines;
            string facultyName = "";

            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name;
                int userFaculty = _unitOfWork.User.Get(x => x.Email == userEmail)?.FacultyId ?? 0;

                if (userFaculty != 0)
                {
                    facultyName = _unitOfWork.Faculty.Get(x => x.Id == userFaculty)?.Name ?? "";
                    List<Magazine> magazineList = _unitOfWork.Magazine.GetAll(filter: x => x.FacultyId == userFaculty, includeProperties: "Faculty,Semester").ToList();

                    closedMagazines = magazineList.Where(m => m.EndDate <= DateTime.Now).ToList();
                    openMagazines = magazineList.Where(m => m.EndDate > DateTime.Now).ToList();
                }
                else
                {

                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                openMagazines = _unitOfWork.Magazine.GetAll().Where(m => m.EndDate > DateTime.Now).ToList();
                closedMagazines = _unitOfWork.Magazine.GetAll().Where(m => m.EndDate <= DateTime.Now).ToList();
            }

            return View(new Tuple<List<Magazine>, List<Magazine>, string>(openMagazines, closedMagazines, facultyName));
        }

        [HttpPost]
        public IActionResult Index(string keyword)
        {
            List<Magazine> searchResult;

            if (!string.IsNullOrEmpty(keyword))
            {
                // Search for magazines containing the keyword in their names
                searchResult = _unitOfWork.Magazine.GetAll()
                    .Where(m => m.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                // If the keyword is empty, return all magazines
                searchResult = _unitOfWork.Magazine.GetAll().ToList();
            }

            // Separate the search results into open and closed magazines
            var openMagazines = searchResult.Where(m => m.EndDate > DateTime.Now).ToList();
            var closedMagazines = searchResult.Where(m => m.EndDate <= DateTime.Now).ToList();

            // Get the faculty name if the user is authenticated
            string facultyName = "";
            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name;
                int userFaculty = _unitOfWork.User.Get(x => x.Email == userEmail)?.FacultyId ?? 0;
                facultyName = userFaculty != 0 ? _unitOfWork.Faculty.Get(x => x.Id == userFaculty)?.Name ?? "" : "";
            }
            else
            {
                // Filter magazines for non-authenticated users based on search string
                List<Magazine> magazineList = _unitOfWork.Magazine.GetAll(
                     filter: x => (string.IsNullOrEmpty(keyword) || x.Name.Contains(keyword)),
                     includeProperties: "Faculty,Semester").ToList();

                closedMagazines = magazineList.Where(m => m.EndDate <= DateTime.Now).ToList();
                openMagazines = magazineList.Where(m => m.EndDate > DateTime.Now).ToList();
            }

            // Return the search results
            return View("Index", new Tuple<List<Magazine>, List<Magazine>, string>(openMagazines, closedMagazines, facultyName));
        }
    }
}
