using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Repository.IRepository;

namespace MagazineCMS.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class FacultyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FacultyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Manager/Faculty
        public async Task<IActionResult> Index()
        {
            var faculties = _unitOfWork.Faculty.GetAll().ToList() ;
            return View(faculties);
        }

        // GET: Manager/Faculty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = _unitOfWork.Faculty.Get(m => m.Id == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // GET: Manager/Faculty/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Manager/Faculty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Faculty.Add(faculty);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the faculty: " + ex.Message);
                }
            }

            return View(faculty);
        }

        // GET: Manager/Faculty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty =  _unitOfWork.Faculty.Get(f=> f.Id == id);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // POST: Manager/Faculty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Faculty faculty)
        {
            if (id != faculty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Faculty.Update(faculty);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Delete Faculty
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var faculty = _unitOfWork.Faculty.Get(f=> f.Id == id);
                if (faculty == null)
                {
                    return Json(new { success = false, message = "Faculty not found." });
                }

                _unitOfWork.Faculty.Remove(faculty);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Faculty deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting faculty: " + ex.Message });
            }
        }


        // GET: DeleteConfirmed Faculty 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = _unitOfWork.Faculty.Get(f=> f.Id == id);
            if (faculty == null)
            {
                return NotFound();
            }

            _unitOfWork.Faculty.Remove(faculty);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        private bool FacultyExists(int id)
        {
            var faculty = _unitOfWork.Faculty.Get(e => e.Id == id);
            return (faculty != null) ? true : false;
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Faculty> faculties = _unitOfWork.Faculty.GetAll().ToList();
            var facultiesWithMagazineCount = faculties.Select(faculty => new
            {
                Faculty = faculty,
                MagazineCount = _unitOfWork.Magazine.GetAll(m => m.Id == faculty.Id).ToList().Count,
                UserCount = _unitOfWork.User.GetAll(u => u.FacultyId == faculty.Id).ToList().Count
            }).ToList();
            return Json(new { data = facultiesWithMagazineCount });
        }

        #endregion
    }
}
