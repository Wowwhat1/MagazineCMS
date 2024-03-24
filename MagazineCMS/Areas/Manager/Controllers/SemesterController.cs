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
            return View();
        }



        #region API CALLS

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

                return View(semester);
            }
            return View(semester);
        }

        #endregion
    }
}
