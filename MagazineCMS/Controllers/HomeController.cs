using Microsoft.AspNetCore.Mvc;

namespace MagazineCMS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
