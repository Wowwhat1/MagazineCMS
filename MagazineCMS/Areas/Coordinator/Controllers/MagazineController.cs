using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    [Authorize(Roles = SD.Role_Coordinator)]

    public class MagazineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MagazineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            string userEmail = User.Identity.Name;
            var faculty = _unitOfWork.User.Get(u => u.Email == userEmail, includeProperties: "Faculty").Faculty;
            List<Magazine> magazineList = _unitOfWork.Magazine.GetAll(filter: x => x.FacultyId == faculty.Id, includeProperties: "Faculty,Semester").ToList();
            List<Magazine> closedMagazines = magazineList.Where(m => m.EndDate <= DateTime.Now).ToList();
            List<Magazine> openMagazines = magazineList.Where(m => m.EndDate > DateTime.Now).ToList();
            return View(new Tuple<List<Magazine>, List<Magazine>, string>(openMagazines, closedMagazines, faculty.Name));
        }

        public IActionResult Details(int id)
        {
            var magazine = _unitOfWork.Magazine.Get(m => m.Id == id, includeProperties: "Faculty,Semester");
            var contributions = _unitOfWork.Contribution.GetAll( c =>
                c.MagazineId == id,
                includeProperties: "Documents,User"
                ).ToList();
            return View(new Tuple<Magazine, List<Contribution>>(magazine, contributions));
        }
    }
}
