using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = SD.Role_Manager)]
    public class SemesterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SemesterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(new Semester());
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetById(int id)
        {
            Semester semester = _unitOfWork.Semester.Get(s=> s.Id == id);
            return Json(new { data = semester });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Semester> semesterList = _unitOfWork.Semester.GetAll().ToList();
            return Json(new { data = semesterList });
        }

        [HttpPost]
        public IActionResult Index(Semester semester)
        {
            if (ModelState.IsValid)
            {
                if (semester.Id == 0)
                {
                    _unitOfWork.Semester.Add(semester);
                    TempData["success"] = "Semester created successfully";
                }
                else
                {
                    _unitOfWork.Semester.Update(semester);
                    TempData["success"] = "Semester updated successfully";
                }
                _unitOfWork.Save();

                return View(new Semester());
            }
            return View(new Semester());
        }

        [HttpDelete]
        public IActionResult DeleteById(int id)
        {
            Semester semester = _unitOfWork.Semester.Get(s => s.Id == id);
            Magazine canDelete = _unitOfWork.Magazine.Get(Magazine => Magazine.SemesterId == id);
            if (semester == null)
            {
                return BadRequest(new { success = false, message = "Error while deleting semester" });
            }
            else if (canDelete != null)
            {
                return BadRequest(new { success = false, message = "The Semester is being used by a magazine" });
            }
            _unitOfWork.Semester.Remove(semester);
            _unitOfWork.Save();
            return Ok(new { success = true, message = "Delete semester: \"" + semester.Name + "\" successful" });

        }

        #endregion
    }
}
