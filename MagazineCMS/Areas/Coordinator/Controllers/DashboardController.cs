using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagazineCMS.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    [Authorize(Roles = SD.Role_Coordinator)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var user = User.Identity.Name;
            var facultyId = _unitOfWork.User.Get(u => u.UserName == user).FacultyId;
    
            DateTime currentDate = DateTime.Now;
            var currentSemester = _unitOfWork.Semester.Get(s => s.StartDate <= currentDate && currentDate <= s.EndDate);
            if (currentSemester == null)
            {
                //get all close semester
                var semesters = _unitOfWork.Semester.GetAll(s => s.EndDate <= currentDate ).ToList();
                currentSemester = semesters[semesters.Count -1];
            }
            var magazine = _unitOfWork.Magazine.Get(m => m.SemesterId == currentSemester.Id && m.FacultyId == facultyId, includeProperties: "Faculty,Semester");

            var contributions = _unitOfWork.Contribution.GetAll(c => c.MagazineId == magazine.Id).ToList();
            var countContributionApproved = contributions.Count(c => c.Status == SD.Status_Approved || c.Status == SD.Status_Public);
            var countContributionPending = contributions.Count(c => c.Status == SD.Status_Pending);
            var countContributionRejected = contributions.Count(c => c.Status == SD.Status_Rejected);

            var magazineSelectList = new SelectList(_unitOfWork.Magazine.GetAll(m=> m.FacultyId == facultyId).ToList(), "Id", "Name");
            ViewBag.MagazineSelectList = magazineSelectList;

            var magazines = _unitOfWork.Magazine.GetAll(m => m.FacultyId == facultyId, includeProperties: "Semester,Faculty")
                .OrderByDescending(m => m.EndDate)
                .Take(6)
                .ToList(); 
            return View(new Tuple<Magazine, int, int, int, List<Magazine>>(magazine, countContributionApproved, countContributionPending, countContributionRejected, magazines));
        }

        public IActionResult GetCardInfo(int id)
        {
            var magazine = _unitOfWork.Magazine.Get(m => m.Id == id, includeProperties: "Faculty,Semester");

            // Calculate the updated card info based on the selected magazine
            var contributions = _unitOfWork.Contribution.GetAll(c => c.MagazineId == magazine.Id).ToList();
            var countContributionApproved = contributions.Count(c => c.Status == SD.Status_Approved);
            var countContributionPending = contributions.Count(c => c.Status == SD.Status_Pending);
            var countContributionRejected = contributions.Count(c => c.Status == SD.Status_Rejected);

            return PartialView("_DashboardCard", new Tuple<Magazine, int, int, int>(magazine, countContributionApproved, countContributionPending, countContributionRejected));
        }
    }
}
