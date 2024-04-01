using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagazineCMS.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contributions = await _context.Contributions.Include(c => c.Magazine)
                .ThenInclude(m => m.Faculty).GroupBy(c => c.Magazine.FacultyId)
                .Select(g => new { FacultyName = g.Select(c => c.Magazine.Faculty.Name)
                .FirstOrDefault(), Count = g.Count() }).ToListAsync();
            return View(contributions);
        }
    }
}
