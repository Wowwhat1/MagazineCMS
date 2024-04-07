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
        /* public async Task<IActionResult> Index()
         {
             var faculties = _unitOfWork.Faculty.GetAll().ToList() ;
             return View(faculties);
         }*/

        public IActionResult Index(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Faculty());
            }
            Faculty faculty = _unitOfWork.Faculty.Get(s => s.Id == id);
            return View(faculty);
        }

        // GET: Manager/Faculty/Details/5
        /*      public async Task<IActionResult> Details(int? id)
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
      */
        // GET: Manager/Faculty/Create
        /*        public IActionResult Create()
                {
                    return View();
                }*/


        // GET: Manager/Faculty/Edit/5
        /*        public async Task<IActionResult> Edit(int? id)
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
        */
        // POST: Manager/Faculty/Edit/5
        /*        [HttpPost]
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
                }*/

        // GET: Delete Faculty
        /*     [HttpGet]
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
             }*/


        // GET: DeleteConfirmed Faculty 
        /* public async Task<IActionResult> DeleteConfirmed(int id)
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
         }*/


        #region API CALLS


        /*  [HttpGet]
          public IActionResult GetById(int id)
          {
              Faculty faculty = _unitOfWork.Faculty.Get(s => s.Id == id);
              return Json(new { data = faculty });
          }*/

        [HttpGet]
        public IActionResult GetById(int id)
        {
            Faculty faculty = _unitOfWork.Faculty.Get(s => s.Id == id);
            if (faculty == null)
            {
                return NotFound(); // Trả về mã trạng thái 404 nếu không tìm thấy Faculty
            }

            // Lấy số lượng tạp chí và số lượng người dùng của faculty này
            int magazineCount = _unitOfWork.Magazine.GetAll(m => m.Id == id).ToList().Count;
            int userCount = _unitOfWork.User.GetAll(u => u.FacultyId == id).ToList().Count;

            // Tạo một đối tượng mới chứa thông tin faculty và số lượng tạp chí, số lượng người dùng
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
                MagazineCount = _unitOfWork.Magazine.GetAll(m => m.Id == faculty.Id).ToList().Count,
                UserCount = _unitOfWork.User.GetAll(u => u.FacultyId == faculty.Id).ToList().Count
            }).ToList();
            return Json(new { data = facultiesWithMagazineCount });
        }

        /*[HttpGet]
        public IActionResult GetAll()
        {
            List<Faculty> facultyList = _unitOfWork.Faculty.GetAll().ToList();
            return Json(new { data = facultyList });
        }*/

        /* [HttpPost]
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

                 return View(faculty);
             }
             return View(faculty);
         }*/

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
                var facultiesWithMagazineCount = faculties.Select(f => new
                {
                    Faculty = faculty,
                    MagazineCount = _unitOfWork.Magazine.GetAll(m => m.Id == faculty.Id).ToList().Count,
                    UserCount = _unitOfWork.User.GetAll(u => u.FacultyId == faculty.Id).ToList().Count
                }).ToList();

                return Json(new { data = facultiesWithMagazineCount });
            }
            return View(faculty);
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
