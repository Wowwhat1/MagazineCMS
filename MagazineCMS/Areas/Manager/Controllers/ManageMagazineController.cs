using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace MagazineCMS.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ManageMagazineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public ManageMagazineController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
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

                var semester = _unitOfWork.Semester.Get(s => s.Id == magazine.SemesterId);

                if (magazine.StartDate >= semester.StartDate && magazine.EndDate <= semester.EndDate)
                {
                    _unitOfWork.Magazine.Add(entity: magazine);
                    _unitOfWork.Save();

                    TempData["Success"] = "Magazine created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The Magazine's StartDate must be greater than the Semester's StartDate and the Magazine's EndDate must be less than the Semester's EndDate.");
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
                    _unitOfWork.Magazine.Update(magazine);
                    _unitOfWork.Save();
                    TempData["Success"] = "Magazine updated successfully";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["Error"] = "Failed to update magazine.";
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

        public IActionResult Details(int id)
        {
            var magazine = _unitOfWork.Magazine.Get(m => m.Id == id, includeProperties: "Faculty,Semester");
            var contributions = _unitOfWork.Contribution.GetAll(c =>
                c.MagazineId == id,
                includeProperties: "Documents,User"
                ).ToList();
            return View(new Tuple<Magazine, List<Contribution>>(magazine, contributions));
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

        public async Task<List<Document>> GetDocumentsByMagazineId(int magazineId)
        {
            var contributions = await _context.Contributions
                .Include(c => c.Documents)
                .Where(c => c.MagazineId == magazineId)
                .ToListAsync();

            var documents = new List<Document>();
            foreach (var contribution in contributions)
            {
                documents.AddRange(contribution.Documents);
            }

            return documents;
        }


        [HttpGet]
        public async Task<IActionResult> DownloadAllDocuments(int magazineId)
        {
            // Get all Documents for this Magazine
            var documents = await GetDocumentsByMagazineId(magazineId);

            // Create a new zip archive in memory
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var document in documents)
                    {
                        // Get the Document's file path
                        var path = Path.Combine("Documents", document.Contribution.UserId, document.DocumentUrl);

                        // Get the Document's file name
                        var fileName = Path.GetFileName(path);

                        // Add the Document to the zip archive
                        var zipEntry = archive.CreateEntry(fileName);

                        // Copy the Document's contents to the zip entry
                        using (var originalFileStream = System.IO.File.OpenRead(path))
                        using (var zipEntryStream = zipEntry.Open())
                        {
                            await originalFileStream.CopyToAsync(zipEntryStream);
                        }
                    }
                }

                // Return the zip archive as a download
                return File(memoryStream.ToArray(), "application/zip", "Documents.zip");
            }
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

            foreach (var magazine in magazineList)
            {
                magazine.ContributionCount = _unitOfWork.Contribution.GetAll(c => c.MagazineId == magazine.Id).Count();

                // Get all Contributions for this Magazine
                var contributions = _unitOfWork.Contribution.GetAll(c => c.MagazineId == magazine.Id);

                // Initialize a counter for the Documents
                int documentCount = 0;

                // For each Contribution, get all Documents and add their count to the counter
                foreach (var contribution in contributions)
                {
                    documentCount += _unitOfWork.Document.GetAll(d => d.ContributionId == contribution.Id).Count();
                }

                // Add the Document count to the Magazine
                magazine.DocumentCount = documentCount;
            }

            return Json(new { data = magazineList, closedMagazines, openMagazines });
        }


        [HttpGet]
        public IActionResult GetSemester()
        {
            List<Semester> semestersList = _unitOfWork.Semester.GetAll().ToList();
            List<Semester> closedSemester = semestersList.Where(m => m.EndDate <= DateTime.Now).ToList();
            List<Semester> openSemester = semestersList.Where(m => m.EndDate > DateTime.Now).ToList();

            return Json(new { data = semestersList, closedSemester, openSemester });
        }

        [HttpDelete, ActionName("deleteMagazine")]
        public IActionResult DeletePOST(int? id)
        {
            //_logger.LogError("Error occurred while deleting magazine" + id);
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


        [HttpPost]
        public IActionResult UpdateContributionStatus(int[] contributionIds)
        {
            if (contributionIds == null || contributionIds.Length == 0)
            {
                return BadRequest(new { success = false, message = "No contribution selected" });
            }
            var selectedContributions = _unitOfWork.Contribution.GetAll(c => contributionIds.Contains(c.Id));
            foreach (var contribution in selectedContributions)
            {
                contribution.Status = SD.Status_Public;
                _unitOfWork.Contribution.Update(contribution);
            }
            _unitOfWork.Save();
            TempData["Success"] = "Contributions public successfully";
            return Ok();
        }

        #endregion
    }
}
