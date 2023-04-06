﻿using RealSys.CoreLib.Interfaces;
using RealSys.CoreLib.Models.Reports;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.SysDB;

namespace RealSys.Modules.Reports
{
    public class ReportRepo:IReportRepo
    {
        private readonly SysDBContext _context;
        public ReportRepo(SysDBContext context)
        {
            this._context = context;
        }
        public IList<Report> GetAvailableReportsByIds(IList<int> rptIds)
        {
            return _context.Reports
                .Include(i=>i.RptReportCats).ThenInclude(i=>i.RptCategory)
                //.Include(i=>i.RptReportUsers.Where(a=>a.AspNetUserId==id))
                .Where( s=> rptIds.Contains (s.Id) )
                .ToList();

        }

        //public IList<int>? _getUserReportsByRoleIds(IList<int> roleIds)
        //{
        //    return _context.rptReportRoles.Where(d=> roleIds.Contains (d.Id))
        //        .Select(s=>s.Id).ToList() as IList<int>;
        //}
        public IList<int>? GetUserReportsByUsername(string userName)
        {
            return _context.rptReportUsers.Where(d => d.AspNetUserId == userName)
                .Select(s=>s.ReportId).ToList();
        }
        public IList<int>? GetDemoReports()
        {
            var democat = _context.rptCategories.Where(d => d.Code == "DEMO").FirstOrDefault();
            int idemoId = (democat==null ? 0 : democat.Id);
            var retList = _context.rptReportCats.Where(d=>d.RptCategoryId==idemoId)
                .Select(s => s.ReportId).ToList();

            if(retList==null) return new List<int>();

            return retList;
        }

        public IQueryable<RptCategory> rptCategories 
        {
            get
            {
                return this._context.rptCategories;
            }
        }
    }
}