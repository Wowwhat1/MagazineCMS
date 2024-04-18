using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;

namespace MagazineCMS.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = SD.Role_Manager)]

    public class FacultyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FacultyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Faculty());
            }
            Faculty faculty = _unitOfWork.Faculty.Get(s => s.Id == id);
            return View(faculty);
        }

        #region API CALLS


        [HttpGet]
        public IActionResult GetById(int id)
        {
            Faculty faculty = _unitOfWork.Faculty.Get(s => s.Id == id);
            if (faculty == null)
            {
                return NotFound(); // Returns a 404 status code if Faculty is not found
            }

            // Get the number of magazines and the number of users of this faculty
            int magazineCount = _unitOfWork.Magazine.GetAll(m => m.Id == id).ToList().Count;
            int userCount = _unitOfWork.User.GetAll(u => u.FacultyId == id).ToList().Count;

            // Create a new object containing faculty information and number of magazines, number of users
            var facultyWithCounts = new
            {
                Faculty = faculty,
                MagazineCount = magazineCount,
                UserCount = userCount
            };

            return Json(new { data = facultyWithCounts });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Faculty> faculties = _unitOfWork.Faculty.GetAll().ToList();
            var facultiesWithMagazineCount = faculties.Select(faculty => new
            {
                Faculty = faculty,
                MagazineCount = _unitOfWork.Magazine.GetAll(m => m.FacultyId == faculty.Id).ToList().Count,
                UserCount = _unitOfWork.User.GetAll(u => u.FacultyId == faculty.Id).ToList().Count
            }).ToList();
            return Json(new { data = facultiesWithMagazineCount });
        }

        [HttpPost]
        public IActionResult Index(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                if (faculty.Id == 0)
                {
                    _unitOfWork.Faculty.Add(faculty);
                    TempData["success"] = "Faculty created successfully";
                }
                else
                {
                    _unitOfWork.Faculty.Update(faculty);
                    TempData["success"] = "Faculty updated successfully";
                }
                _unitOfWork.Save();

                // Sau khi thêm hoặc cập nhật Faculty, chúng ta cần cập nhật lại dữ liệu để hiển thị
                List<Faculty> faculties = _unitOfWork.Faculty.GetAll().ToList();
                var facultiesWithMagazineCount = faculties.Select(faculty => new
                {
                    Faculty = faculty,
                    MagazineCount = _unitOfWork.Magazine.GetAll(m => m.Id == faculty.Id).ToList().Count,
                    UserCount = _unitOfWork.User.GetAll(u => u.FacultyId == faculty.Id).ToList().Count
                }).ToList();

            }
            return RedirectToAction("Index", "Faculty");
        }

        [HttpDelete]
        public IActionResult DeleteById(int id)
        {
            Faculty faculty = _unitOfWork.Faculty.Get(s => s.Id == id);
            Magazine canDelete = _unitOfWork.Magazine.Get(Magazine => Magazine.FacultyId == id);
            if (faculty == null)
            {
                return BadRequest(new { success = false, message = "Error while deleting faculty" });
            }
            else if (canDelete != null)
            {
                return BadRequest(new { success = false, message = "The Faculty is being used by a magazine" });
            }
            _unitOfWork.Faculty.Remove(faculty);
            _unitOfWork.Save();
            return Ok(new { success = true, message = "Delete faculty: \"" + faculty.Name + "\" successful" });

        }

        #endregion
    }
}
