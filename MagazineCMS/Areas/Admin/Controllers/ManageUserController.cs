using System.Text.Encodings.Web;
using System.Text;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MagazineCMS.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace MagazineCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

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
            UserVM userVM = CreateUserVM();
           
            return View(userVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserVM userVM)
        { 
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Firstname = userVM.User.Firstname;
                user.Lastname = userVM.User.Lastname;
                user.AvatarUrl = userVM.User.AvatarUrl;
                user.FacultyId = (int)userVM.User.FacultyId;
                user.Email = userVM.User.Email;
                user.UserName = userVM.User.Email;
                user.AvatarUrl = SD.Default_Avatar;

                var result = await _userManager.CreateAsync(user, userVM.Password);

                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(userVM.User.Role))
                    {
                        await _userManager.AddToRoleAsync(user, userVM.User.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Student);
                    }
                    TempData["Success"] = "User created successfully";
                    return Redirect("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            UserVM newUserVM = CreateUserVM();
            newUserVM.User = userVM.User;
            TempData["Error"] = "Error creating user";
            return View(newUserVM);
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        public IActionResult Edit(int? id)
        {
            return View();
        }

        private UserVM CreateUserVM()
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
            return userVM;
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

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                // User is currently locked and we need to unlock them
                user.LockoutEnd = null;
            }
            else
            {
                // User is not locked, so lock them
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(10);
            }

            var result = _userManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                _unitOfWork.Save();
                return Json(new { success = true, message = "Operation Successful" });
            }
            else
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Delete the user
            var deleteResult = await _userManager.DeleteAsync(user);

            if (deleteResult.Succeeded)
            {
                _unitOfWork.Save();
                return Ok(new { success = true, message = "User deleted successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error while deleting user" });
            }
        }

        #endregion
    }
}
