using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MagazineCMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;


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

        public IActionResult Magazine(int id)
        {
            var magazine = _unitOfWork.Magazine.Get(x => x.Id == id, includeProperties: "Faculty,Semester");

            // Fetch all contributions for the selected magazine
            var contributions = _unitOfWork.Contribution.GetAll(
                filter: c => c.MagazineId == id,
                includeProperties: "Documents,User");

            // Pass both magazine and contributions to the view
            var tuple = new Tuple<Magazine, IEnumerable<Contribution>>(magazine, contributions);
            return View(tuple);
        }

        public IActionResult GetContribution()
        {
            var contributions = _unitOfWork.Contribution.GetAll(includeProperties: "User, Magazine").ToList();
            var openContributions = contributions.Where(contributions => contributions.Magazine.EndDate > DateTime.Now).ToList();
            var closeContributions = contributions.Where(contributions => contributions.Magazine.EndDate > DateTime.Now).ToList();
            foreach (var contribution in contributions)
            {
                contribution.Magazine.Faculty = _unitOfWork.Faculty.Get(f => f.Id == contribution.Magazine.FacultyId);
                contribution.Magazine.Semester = _unitOfWork.Semester.Get(s => s.Id == contribution.Magazine.SemesterId);
            }
            foreach (var contribution in contributions)
            {
                if (contribution.Magazine.EndDate > DateTime.Now)
                {
                    if (!openContributions.Contains(contribution))
                    {
                        openContributions.Add(contribution);
                    }
                    if (closeContributions.Contains(contribution))
                    {
                        closeContributions.Remove(contribution);
                    }
                }
                else
                {
                    if (!closeContributions.Contains(contribution))
                    {
                        closeContributions.Add(contribution);
                    }
                    if (openContributions.Contains(contribution))
                    {
                        openContributions.Remove(contribution);
                    }
                }
            }

            return View(new Tuple<List<Contribution>, List<Contribution>>(openContributions, closeContributions));
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

        public IActionResult Contribution(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contribution contribution = _unitOfWork.Contribution.Get(c => c.Id == id, includeProperties: "Documents,User");
            if (contribution == null || contribution.Status != SD.Status_Public)
            {
                return NotFound();
            }
            Magazine magazine = _unitOfWork.Magazine.Get(m => m.Id == contribution.MagazineId, includeProperties: "Semester,Faculty");

            return View(new Tuple<Contribution, Magazine>(contribution, magazine));
        }
    }
}