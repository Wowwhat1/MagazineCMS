using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contribution(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contribution contribution = _unitOfWork.Contribution.Get(c => c.Id == id, includeProperties: "Documents,User");
            if (contribution == null || contribution.Status != SD.Status_Public)
            {
                return NotFound();
            }
            Magazine magazine = _unitOfWork.Magazine.Get(m => m.Id == contribution.MagazineId, includeProperties: "Semester,Faculty");

            return View(new Tuple<Contribution, Magazine>(contribution, magazine));
        }
    }
}
