using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    [Authorize(Roles = SD.Role_Coordinator)]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var countContributionApproved = 0;
            var countContributionPending = 0;
            var countContributionRejected = 0;

            DateTime currentDate = DateTime.Now;
            var currentSemester = _unitOfWork.Semester.Get(s => s.StartDate <= currentDate && currentDate <= s.EndDate);
            if (currentSemester == null)
            {
                //get all close semester
                var semesters = _unitOfWork.Semester.GetAll(s => s.EndDate <= currentDate ).ToList();
                currentSemester = semesters[semesters.Count -1];
            }
            // get contributions
            var magazineIds = _unitOfWork.Magazine.GetAll(m => m.SemesterId == currentSemester.Id).Select(m => m.Id).ToList();
            foreach (var magazineId in magazineIds)
            {
                var a = _unitOfWork.Contribution.GetAll(c => c.MagazineId == magazineId && c.Status == SD.Status_Approved).ToList().Count;
                var b = _unitOfWork.Contribution.GetAll(c => c.MagazineId == magazineId && c.Status == SD.Status_Pending).ToList().Count;
                var c = _unitOfWork.Contribution.GetAll(c => c.MagazineId == magazineId && c.Status == SD.Status_Rejected).ToList().Count;
                countContributionApproved += a;
                countContributionPending += b;
                countContributionPending += c;       
            }
            return View(new Tuple<Semester, int, int, int>(currentSemester, countContributionApproved, countContributionPending, countContributionRejected));
        }
    }
}
