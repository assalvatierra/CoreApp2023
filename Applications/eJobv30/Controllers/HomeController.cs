using eJobv30.Models;
using Microsoft.AspNetCore.Mvc;
using RealSys.CoreLib.Interfaces.System;
using System.Diagnostics;


namespace eJobv30.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISystemServices _systemservices;

        public HomeController(ILogger<HomeController> logger, ISystemServices sysservices)
        {
            _logger = logger;
            this._systemservices = sysservices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Modules()
        {
            var sysItems = this._systemservices.getServices(0).ToList();
            ViewBag.sysItems = sysItems;

            return View();
        }
        public IActionResult route(int Id)
        {
            return View();
        }



    }
}