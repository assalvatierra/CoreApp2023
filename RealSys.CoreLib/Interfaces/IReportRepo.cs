//using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealSys.CoreLib.Models.Reports;

namespace RealSys.CoreLib.Interfaces
{
    public interface IReportRepo
    {
        public IList<Report> GetAvailableReportsByIds(IList<int> rptIds);
        public IList<int>? GetUserReportsByUsername(string userName);
        //public IList<int>? GetUserReportsByRoleIds(IList<int> roleIds);

        public IQueryable<RptCategory> rptCategories { get; }


    }
}
