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
            List<int> rptIDs = new List<int>();

            if (userName==null)
            {
                IList<int> rptDemos = this._reportsRepo.GetDemoReports();
                rptIDs.AddRange(rptDemos);
            }

            IList<int> rptByUser = this._reportsRepo.GetUserReportsByUsername(userName);
            rptIDs.AddRange(rptByUser);

            //var rptByRole = this._reportsRepo.GetUserReportsByRoleIds(userRoleIds);
            //rptIDs.AddRange(rptByRole);


            return this._reportsRepo.GetAvailableReportsByIds(rptIDs);
        }

        public IList<RptCategory> GetAvailableCategories()
        {
            return this._reportsRepo.rptCategories.ToList();
        }

    }
}
