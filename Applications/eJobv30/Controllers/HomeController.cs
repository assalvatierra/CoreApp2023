using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using eJobv30.Models;
using Microsoft.AspNetCore.Mvc;
using RealSys.CoreLib.Interfaces.System;
using System.Diagnostics;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ReportDesigner;
using eJobv30.Reporting.Models;
using DevExpress.Web;
using RealSys.CoreLib.Models.SysDB;

namespace eJobv30.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISystemServices _systemservices;
        private SysDBContext sysDBContext;

        public HomeController(ILogger<HomeController> logger, ISystemServices sysservices, SysDBContext _sysDBContext)
        {
            _logger = logger;
            this._systemservices = sysservices;
            sysDBContext = _sysDBContext;
        }

        public IActionResult Index()
        {
            List<RealSys.Modules.SysLib.Models.MenuItem> MenuItem = new List<RealSys.Modules.SysLib.Models.MenuItem>();
            MenuItem.Add( 
                new RealSys.Modules.SysLib.Models.MenuItem() { 
                    Id=1,OrderNo=1,MenuName="Privacy",Route="Home/Privacy"
            } );

            MenuItem = GetMenuList(MenuItem);

            ViewData["MenuItems"] = MenuItem;
            return View();
        }


        public List<RealSys.Modules.SysLib.Models.SubMenuItem> GetSubMenuItems(List<SysMenu> MenuList, int MenuId)
        {

            List<RealSys.Modules.SysLib.Models.SubMenuItem> SubMenuItems = new List<RealSys.Modules.SysLib.Models.SubMenuItem>();

            MenuList.Where(c => c.ParentId == MenuId).ToList().ForEach(m => {
                var param = m.Params == null ? "" : "?" + m.Params;

                SubMenuItems.Add(
                  new RealSys.Modules.SysLib.Models.SubMenuItem()
                  {
                      Id = m.Id,
                      OrderNo = m.Seqno,
                      MenuName = m.Menu,
                      Route = m.Controller + "/" + m.Action + param

                  });
            });

            if (SubMenuItems.Count == 0)
            {
                return null;
            }

            return SubMenuItems;
        }

        public List<RealSys.Modules.SysLib.Models.MenuItem> GetMenuList(List<RealSys.Modules.SysLib.Models.MenuItem> MenuItem )
        {

            var MenuListIds = sysDBContext.SysAccessUsers.Where(s => s.UserId == User.Identity.Name).Select(s => s.SysMenuId);
            var MenuList = sysDBContext.SysMenus.Where(s => MenuListIds.Contains(s.Id)).OrderBy(c => c.Seqno).ToList();

                MenuList.Where(c => c.ParentId == 0).ToList().ForEach(m =>
                {
                    var param = m.Params == null ? "" : "?" + m.Params;

                    MenuItem.Add(
                       new RealSys.Modules.SysLib.Models.MenuItem()
                       {
                           Id = m.Id,
                           OrderNo = m.Seqno,
                           MenuName = m.Menu,
                           MenuNameHTMLId = m.Menu.Replace(' ', '-'),
                           Route = m.Controller + "/" + m.Action + param,
                           //SubMenuItems = GetSubMenuItems(MenuList, m.Id)

                       });
                });

            return MenuItem;
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
            string sPath = this._systemservices.getModuleLink(Id);
            var protocol = HttpContext.Request.Scheme;
            var domain = HttpContext.Request.Host.Value;
            //var port = HttpContext.Request.Host.Port;

            string sLink = protocol + "://" + domain + sPath;
            return Redirect(sLink);
            //return View();
        }
        public async Task<IActionResult> Viewer(
        [FromServices] IWebDocumentViewerClientSideModelGenerator clientSideModelGenerator,
        [FromQuery] string reportName)
        {
            // add temporary menu items
            List<RealSys.Modules.SysLib.Models.MenuItem> MenuItem = new List<RealSys.Modules.SysLib.Models.MenuItem>();
            MenuItem.Add(
                new RealSys.Modules.SysLib.Models.MenuItem()
                {
                    Id = 1,
                    OrderNo = 1,
                    MenuName = "Privacy",
                    Route = "Home/Privacy"
                });


            var reportToOpen = string.IsNullOrEmpty(reportName) ? "TestReport" : reportName;
            //var reportToOpen = "ItemList";

            var model = new ViewerModel
            {
                ViewerModelToBind = await clientSideModelGenerator.GetModelAsync(reportToOpen, WebDocumentViewerController.DefaultUri)
            };
            return View(model);
        }



        public async Task<IActionResult> Designer(
            [FromServices] IReportDesignerClientSideModelGenerator clientSideModelGenerator,
            [FromQuery] string reportName)
        {
            eJobv30.Reporting.Models.ReportDesignerCustomModel model = new eJobv30.Reporting.Models.ReportDesignerCustomModel();
            model.ReportDesignerModel = await CreateDefaultReportDesignerModel(clientSideModelGenerator, reportName, null);
            return View(model);
        }

        public static Dictionary<string, object> GetAvailableDataSources()
        {
            var dataSources = new Dictionary<string, object>();
            // Create a SQL data source with the specified connection string.
            SqlDataSource ds = new SqlDataSource("ReportsMssqlServer");
            // Create a SQL query to access the Products data table.
            SelectQuery query = SelectQueryFluentBuilder.AddTable("InvCategories").SelectAllColumnsFromTable().Build("Reports");
            ds.Queries.Add(query);
            ds.RebuildResultSchema();
            dataSources.Add("InventoryDB", ds);
            return dataSources;
        }

        public static async Task<ReportDesignerModel> CreateDefaultReportDesignerModel(IReportDesignerClientSideModelGenerator clientSideModelGenerator, string reportName, XtraReport report)
        {
            reportName = string.IsNullOrEmpty(reportName) ? "TestReport" : reportName;
            var dataSources = GetAvailableDataSources();
            if (report != null)
            {
                return await clientSideModelGenerator.GetModelAsync(report, dataSources, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
            }
            return await clientSideModelGenerator.GetModelAsync(reportName, dataSources, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
        }




    }
}