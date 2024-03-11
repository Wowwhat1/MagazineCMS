using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Create()
        {
            UserVM userVM = new UserVM()
            {
                User = new User(),
                FacultyList = _unitOfWork.Faculty
                .GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                RoleList = _roleManager.Roles
                    .Where(role => role.Name != SD.Role_Admin)
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Name
                    })
            };
            return View(userVM);
        }
        [HttpPost]
        public IActionResult Create(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var user = userVM.User;
                user.AvatarUrl = SD.Default_Avatar;
                _unitOfWork.User.Add(userVM.User);
                _unitOfWork.Save();
                _userManager.CreateAsync(userVM.User, userVM.Password);
                _userManager.AddToRoleAsync(userVM.User, userVM.Role);
            }
            return Redirect("Index");
        }

        public IActionResult Edit(int? id)
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
