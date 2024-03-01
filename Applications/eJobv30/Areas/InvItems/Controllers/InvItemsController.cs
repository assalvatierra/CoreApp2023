using Microsoft.AspNetCore.Mvc;
using RealSys.CoreLib.Interfaces.System;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using RealSys.Modules.CustomersLib.Lib;
using RealSys.Modules.SysLib;

namespace eJobv30.Areas.InvItems.Controllers
{
    [Area("InvItems")]
    public class InvItemsController : Controller
    {
        private ErpDbContext db;

        private ISystemServices systemservices;

        public InvItemsController(ILogger<InvItemsController> logger, ErpDbContext erpDb, SysDBContext sysDBContext)
        {
            db = erpDb;
            systemservices = new SystemServices(sysDBContext);
        }
        public IActionResult Index()
        {
            return RedirectToAction("IndexDx");
        }
        public IActionResult IndexDx()
        {
            return View("IndexDx");
        }
    }
}
