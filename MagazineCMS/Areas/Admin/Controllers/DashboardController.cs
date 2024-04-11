using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
