using Microsoft.AspNetCore.Mvc;

namespace Sample102.Controllers
{
    public class SampleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
