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
            string sLink = this._systemservices.getModuleLink(Id);

            return View();
        }
        public async Task<IActionResult> Viewer(
        [FromServices] IWebDocumentViewerClientSideModelGenerator clientSideModelGenerator,
        [FromQuery] string reportName)
        {

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