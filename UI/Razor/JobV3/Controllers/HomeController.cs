using Microsoft.AspNetCore.Mvc;

namespace JobV3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sample()
        {
            return View(); ;
        }
    }
}
