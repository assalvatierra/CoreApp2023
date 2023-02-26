using Microsoft.AspNetCore.Mvc;

namespace BlazorSvrApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
