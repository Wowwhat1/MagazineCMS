using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    [Authorize(Roles = SD.Role_Coordinator)]
    public class ContributionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContributionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var contributions = _unitOfWork.Contribution.GetAll(includeProperties: "User,Magazine").ToList();
            return View(contributions);
        }


        public IActionResult Details(int id)
        {
            var contribution = _unitOfWork.Contribution.GetFirstOrDefault(x => x.Id == id, includeProperties: "User,Magazine,Documents,Feedbacks");
            return View(contribution);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string status, int contributionId)
        {
            var contribution = await _unitOfWork.Contribution.GetFirstOrDefaultAsync(c => c.Id == contributionId);
            if (contribution == null)
            {
                return NotFound();
            }

            contribution.Status = status;
            _unitOfWork.Save();

            return RedirectToAction("Details", new { id = contributionId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFeedback([FromForm] int contributionId, [FromForm] string comment)
        {
            var contribution = await _unitOfWork.Contribution.GetFirstOrDefaultAsync(c => c.Id == contributionId, includeProperties: "User,Feedbacks");
            if (contribution == null)
            {
                return NotFound();
            }
            var feedback = new Feedback
            {
                Comment = comment,
                FeedbackDate = DateTime.Now,
                ContributionId = contributionId,
                UserId = contribution.UserId // Set user ID for the feedback
            };

            _unitOfWork.Feedback.Add(feedback);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Details", new { id = contributionId });
        }
    }
}
