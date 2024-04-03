using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace MagazineCMS.Controllers
{
    [Area("Student")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _db = db;
            _userManager = userManager;
        }

        /*public IActionResult Index()
        {
            return View();
        }*/

        /*public IActionResult Index()
        {
            string userEmail = User.Identity.Name;
            int userFaculty = _unitOfWork.User.Get(x => x.Email == userEmail).FacultyId;
            string facultyName = _unitOfWork.Faculty.Get(x => x.Id == userFaculty).Name;
            List<Magazine> magazineList = _unitOfWork.Magazine.GetAll(filter: x => x.FacultyId == 2, includeProperties: "Faculty,Semester").ToList();
            List<Magazine> closedMagazines = magazineList.Where(m => m.EndDate <= DateTime.Now).ToList();
            List<Magazine> openMagazines = magazineList.Where(m => m.EndDate > DateTime.Now).ToList();

            return View(new Tuple<List<Magazine>, List<Magazine>, string>(openMagazines, closedMagazines, facultyName));
        }*/

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
                    //facultyName = _unitOfWork.Faculty.Get(x => x.Id == userFaculty).Name;
                    // L?y danh sách t?p chí thu?c khoa c?a ng??i dùng
                    List<Magazine> magazineList = _unitOfWork.Magazine.GetAll(filter: x => x.FacultyId == userFaculty, includeProperties: "Faculty,Semester").ToList();

                    // Tách t?p chí ?ã ?óng và ?ang m?
                    closedMagazines = magazineList.Where(m => m.EndDate <= DateTime.Now).ToList();
                    openMagazines = magazineList.Where(m => m.EndDate > DateTime.Now).ToList();
                }
                else
                {
                    // X? lý tr??ng h?p không tìm th?y khoa cho ng??i dùng
                    // Ví d?: chuy?n h??ng ??n trang l?i
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                // Ng??i dùng ch?a ??ng nh?p, hi?n th? t?t c? t?p chí
                openMagazines = _unitOfWork.Magazine.GetAll(includeProperties: "Faculty,Semester").Where(m => m.EndDate > DateTime.Now).ToList();
                closedMagazines = _unitOfWork.Magazine.GetAll(includeProperties: "Faculty,Semester").Where(m => m.EndDate <= DateTime.Now).ToList();
            }

            return View(new Tuple<List<Magazine>, List<Magazine>, string>(openMagazines, closedMagazines, facultyName));
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


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
