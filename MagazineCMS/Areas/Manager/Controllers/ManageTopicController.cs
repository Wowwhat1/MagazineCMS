using MagazineCMS.DataAccess.Repository;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MagazineCMS.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ManageTopicController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ManageTopicController> _logger;

        public ManageTopicController(IUnitOfWork unitOfWork, ILogger<ManageTopicController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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
        [HttpGet]
        public IActionResult updateMagazine(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazine = _unitOfWork.Magazine.Get(u => u.Id ==id);
            if (magazine == null)
            {
                return NotFound();
            }

            var Magazine = new MagazineVM
            {
                Magazine = magazine,
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

            return View("Edit",Magazine);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,Name,Description,StartDate,EndDate,FacultyId,SemesterId")] Magazine magazine)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation(magazine.Name);
                    _logger.LogInformation(magazine.Id.ToString());
                    _unitOfWork.Magazine.Update(magazine,magazine.Id);
                    _unitOfWork.Save();
                    TempData["Success"] = "Topic updated successfully";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["Error"] = "Failed to update topic.";
                    return BadRequest(new { success = false, message = "Error while updating magazine" });
                }
               
            }

            var Magazine = new MagazineVM
            {
                Magazine = _unitOfWork.Magazine.Get(u => u.Id == magazine.Id),
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

            return View("Index");
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


        [HttpDelete, ActionName("deleteMagazine")]
        public IActionResult DeletePOST(int? id)
        {
            _logger.LogError("Error occurred while deleting magazine" + id);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                Magazine obj = _unitOfWork.Magazine.Get(u => u.Id == id);

                if (obj == null)
                {
                    return NotFound();
                }

                _unitOfWork.Magazine.Remove(obj);
                _unitOfWork.Save();
                return Ok(new { success = true, message = "Magazine deleted successfully" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { success = false, message = "Error while deleting magazine" });
            }
        }






        #endregion
    }
}
