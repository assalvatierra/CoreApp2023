//using InvWeb.Pages.Masterfiles.ItemMaster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using RealSys.CoreLib.Models.Reports;
//using CoreLib.Models.Inventory;
using Microsoft.Extensions.Logging;
//using RealSys.CoreLib.Interfaces.Reports;
using RealSys.CoreLib.Services;

namespace InvWeb.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ReportServices _reportSvc;

        public IList<Report> invReports { get; set; }
        public IList<RptCategory> rptCategories { get; set; }   
        public string userRptLevel { get; set; }
        public IndexModel(ILogger<IndexModel> logger, ReportServices reportSvc)
        {
            _logger = logger;
            _reportSvc = reportSvc;
        }


        public async Task OnGetAsync()
        {
            this.userRptLevel = "USER";
            //this.userRptLevel = "CREATOR";
            //this.userRptLevel = "ADMIN";

            this.rptCategories = this._reportSvc.GetAvailableCategories();
            this.invReports = this._reportSvc.GetAvailableReports(this.HttpContext.User.Identity.Name);

        }
    }
}
