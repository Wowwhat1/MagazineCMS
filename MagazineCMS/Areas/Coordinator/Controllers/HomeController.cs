using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Areas.Coordinator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
