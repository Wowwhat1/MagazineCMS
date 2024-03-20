using MagazineCMS.DataAccess.Repository;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = SD.Role_Student)]
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
