using MagazineCMS.DataAccess.Repository;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ManageTopicController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageTopicController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            string userEmail = User.Identity.Name;
            int userFaculty = _unitOfWork.User.Get(x => x.Email == userEmail).FacultyId;

            List<Magazine> magazineList = _unitOfWork.Magazine.GetAll(includeProperties: "Faculty,Semester").ToList();
            List<Magazine> closedMagazines = magazineList.Where(m => m.EndDate <= DateTime.Now).ToList();
            List<Magazine> openMagazines = magazineList.Where(m => m.EndDate > DateTime.Now).ToList();

            return Json(new { data = magazineList, closedMagazines, openMagazines });
        }

        #endregion
    }
}
