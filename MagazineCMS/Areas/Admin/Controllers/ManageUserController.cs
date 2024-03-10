using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageUserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageUserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<User> usersList = _unitOfWork.User.GetAll(includeProperties: "Faculty");
            return View(usersList);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<User> usersList = _unitOfWork.User.GetAll(includeProperties: "Faculty").ToList();
            return Json(new { data = usersList });
        }

        #endregion
    }
}
