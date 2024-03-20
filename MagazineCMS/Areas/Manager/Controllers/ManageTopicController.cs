using MagazineCMS.DataAccess.Repository;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Create()
        {
            MagazineVM magazineVM = CreateMagazineVM();

            return View(magazineVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MagazineVM magazineVM)
        {
            if (ModelState.IsValid)
            {
                var magazine = CreateMagazine();

                magazine.Name = magazineVM.Magazine.Name;
                magazine.Description = magazineVM.Magazine.Description;
                magazine.StartDate = magazineVM.Magazine.StartDate;
                magazine.EndDate = magazineVM.Magazine.EndDate;
                magazine.FacultyId = (int)magazineVM.Magazine.FacultyId;
                magazine.SemesterId = (int)magazineVM.Magazine.SemesterId;

                var result = await _unitOfWork.Magazine.CreateAsync(magazine);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Topic created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            MagazineVM newMagazineVM = CreateMagazineVM();
            newMagazineVM.Magazine = magazineVM.Magazine;
            TempData["Error"] = "Error creating magazine";
            return View(newMagazineVM);
        }

        private Magazine CreateMagazine()
        {
            try
            {
                return Activator.CreateInstance<Magazine>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Magazine)}'. " +
                    $"Ensure that '{nameof(Magazine)}' is not an abstract class and has a parameterless constructor, or alternatively ");
            }
        }

        private MagazineVM CreateMagazineVM()
        {
            MagazineVM magazineVM = new MagazineVM()
            {
                Magazine = new Magazine(),
                FacultyList = _unitOfWork.Faculty
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                SemesterList = _unitOfWork.Semester
                    .GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
            };
            return magazineVM;
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
