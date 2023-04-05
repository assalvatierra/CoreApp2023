using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealSys.CoreLib.Interfaces;
using RealSys.CoreLib.Models.Reports;

namespace RealSys.CoreLib.Services
{
    public class ReportServices
    {
        private IReportRepo _reportsRepo;
        public ReportServices(IReportRepo reportsRepo) 
        { 
            this._reportsRepo = reportsRepo;
        }

        public IList<Report> GetAvailableReports(string userName)
        {
            var rptByUser = this._reportsRepo.GetUserReportsByUsername(userName);
            //var rptByRole = this._reportsRepo.GetUserReportsByRoleIds(userRoleIds);

            IList<int> rptIDs = (IList<int>)rptByUser;

            return this._reportsRepo.GetAvailableReportsByIds(rptIDs);
        }

        public IList<RptCategory> GetAvailableCategories()
        {
            return this._reportsRepo.rptCategories.ToList();
        }

    }
}
