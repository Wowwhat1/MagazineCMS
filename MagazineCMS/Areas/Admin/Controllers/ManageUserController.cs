using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageUserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<User> usersList = _unitOfWork.User.GetAll(includeProperties: "Faculty").ToList();
            foreach(var user in usersList)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
            }
            return Json(new { data = usersList });
        }

        #endregion
    }
}
