using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.DTO.Activities;
using RealSys.CoreLib.Models.Erp;
using RealSys.Modules.SysLib.Lib;
using System.Data;

namespace RealSys.Modules.ActivitiesLib
{
    public class ActivitiesServices
    {


        private ErpDbContext db ;
        //private DBClasses dbc;
        private DateClass dt;

        public ActivitiesServices(ErpDbContext _dbcontext)
        {
            db = _dbcontext;
            dt = new DateClass();
        }


        public List<CustEntActivity> GetCompanyActivities()
        {

            List<CustEntActivity> activity = new List<CustEntActivity>();

            //sql query with comma separated item list
            string sql =
               @" 
                SELECT * FROM CustEntActivities ;";

            //activity = db.Database.SqlQuery<CustEntActivity>(sql).ToList();

            activity = db.CustEntActivities.ToList();

            return activity;
        }


        public List<SupplierActivity> GetSupplierActivities()
        {

            List<SupplierActivity> activity = new List<SupplierActivity>();

            //sql query with comma separated item list
            string sql =
               @" SELECT * SupplierActivity";

            //activity = db.Database.SqlQuery<SupplierActivity>(sql).ToList();

            activity = db.SupplierActivities.ToList();

            return activity;
        }


        // GET: Supplier Activities
        public IOrderedEnumerable<SupplierActivity> GetSupplierActivitiesUser(string user, DateTime sdate, DateTime edate)
        {
            //get activities of the user
            var companyActivity = db.SupplierActivities
                .Where(c => c.DtActivity.CompareTo(sdate) >= 0 && c.DtActivity.CompareTo(edate) <= 0 && c.Assigned == user)
                .ToList();
            return companyActivity.OrderByDescending(s => s.DtActivity);
        }


        // GET: Supplier Activities
        public IOrderedEnumerable<SupplierActivity> GetSupplierActivitiesAdmin(DateTime sdate, DateTime edate)
        {
            //get activities of all users
            var companyActivity = db.SupplierActivities
                .Where(c => c.DtActivity.CompareTo(sdate) >= 0 && c.DtActivity.CompareTo(edate) <= 0)
                .ToList();
            return companyActivity.OrderByDescending(s => s.DtActivity);
        }

        // GET: Company Activities
        public IOrderedEnumerable<CustEntActivity> GetCompanyActivitiesUser(string user, DateTime sdate, DateTime edate)
        {
            //get activities of the user
            var companyActivity = db.CustEntActivities
                .Include(c => c.CustEntMain)
                .ThenInclude(c => c.CustEntActivities)
                .Include(c => c.CustEntActStatu)
                .Include(c => c.CustEntActActionStatu)
                .Include(c => c.CustEntActActionCode)
                .Where(c => c.Date.CompareTo(sdate) >= 0 && c.Date.CompareTo(edate) <= 0 && c.Assigned == user)
                .ToList();

            return companyActivity.OrderByDescending(s => s.Date);
        }


        // GET: Company Activities
        public IOrderedEnumerable<CustEntActivity> GetCompanyActivitiesAdmin(DateTime sdate, DateTime edate)
        {
            //get activities of all users
            var companyActivity = db.CustEntActivities
                .Include(c=>c.CustEntMain)
                .ThenInclude(c=>c.CustEntActivities)
                .Include(c => c.CustEntActStatu)
                .Include(c => c.CustEntActActionStatu)
                .Include(c => c.CustEntActActionCode)
                .Where(c => c.Date.CompareTo(sdate) >= 0 && c.Date.CompareTo(edate) <= 0)
                .ToList();
            return companyActivity.OrderByDescending(s => s.Date);
        }

        #region Performance Report
        public List<cUserPerformance> GetUserPerformanceReport(DateTime sdate, DateTime edate)
        {
            var tempDate = edate;
            edate = tempDate.AddDays(1);

            List<cUserPerformance> userReport = new List<cUserPerformance>();

            string sql =
               " SELECT Id,	UserName," +
               "         Role = 'NA', "+
               "         Quotation = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Quotation' AND ca.Remarks = 'Quotation Sent' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Meeting = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Meeting' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Sales = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         ProcMeeting = (SELECT COUNT(*) FROM SupplierActivities sa WHERE sa.ActivityType = 'Meeting' AND au.UserName = sa.Assigned AND convert(datetime, sa.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, sa.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         Procurement = (SELECT COUNT(*) FROM SupplierActivities sa WHERE sa.ActivityType = 'Procurement' AND au.UserName = sa.Assigned AND convert(datetime, sa.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, sa.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         JobOrder = (SELECT COUNT(*) FROM SupplierActivities ca WHERE ca.ActivityType = 'Job Order' AND au.UserName = ca.Assigned AND convert(datetime, ca.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         Amount = (SELECT ISNULL(SUM(Amount),0) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         ProcAmount = (SELECT ISNULL(SUM(Amount),0) FROM SupplierActivities ca WHERE ca.ActivityType = 'Job Order' AND au.UserName = ca.Assigned AND convert(datetime, ca.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.DtActivity) < convert(datetime,'" + edate + "') )" +
               "  FROM AspNetUsers au " +

               "  Where UserName NOT IN " +
               " ('jahdielvillosa@gmail.com' ,'jahdielsvillosa@gmail.com', 'assalvatierra@gmail.com', " +
               "  'demo@gmail.com', 'assalvatierra@yahoo.com' )" +

               "  ORDER BY Sales DESC, Meeting DESC, Quotation Desc ;";

            //userReport = db.Database.SqlQuery<cUserPerformance>(sql).ToList();

            userReport = db.cUserPerformances.FromSqlRaw(sql).ToList();



            //Update total meeting Count
            userReport = UpdateTotalMeeting(userReport);

            return userReport;
        }

        //Override
        public List<cUserPerformance> GetUserPerformanceReport(string user, DateTime sdate, DateTime edate)
        {
            var tempDate = edate;
            edate = tempDate.AddDays(1);

            List<cUserPerformance> userReport = new List<cUserPerformance>();

            string sql =
               " SELECT	UserName," +
               "         Quotation = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Quotation' AND ca.Remarks = 'Quotation Sent' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Meeting = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Meeting' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Sales = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         ProcMeeting = (SELECT COUNT(*) FROM SupplierActivities sa WHERE sa.ActivityType = 'Meeting' AND au.UserName = sa.Assigned AND convert(datetime, sa.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, sa.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         Procurement = (SELECT COUNT(*) FROM SupplierActivities ca WHERE ca.ActivityType = 'Procurement' AND au.UserName = ca.Assigned AND convert(datetime, ca.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         JobOrder = (SELECT COUNT(*) FROM SupplierActivities ca WHERE ca.ActivityType = 'Job Order' AND au.UserName = ca.Assigned AND convert(datetime, ca.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         Amount = (SELECT ISNULL(SUM(Amount),0) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )" +
               "  FROM AspNetUsers au " +

               "  Where UserName = '" + user + "' " +

               "  ORDER BY Sales DESC, Meeting DESC, Quotation Desc ;";

            //userReport = db.Database.SqlQuery<cUserPerformance>(sql).ToList();
            userReport = db.cUserPerformances.FromSqlRaw(sql).ToList();

            //Update total meeting Count
            userReport = UpdateTotalMeeting(userReport);

            return userReport;
        }


        private List<cUserPerformance> UpdateTotalMeeting(List<cUserPerformance> userPerf)
        {
            foreach (var perf in userPerf)
            {
                perf.Meeting += perf.ProcMeeting;
            }

            return userPerf;
        }



        public string GetUserRole(string user)
        {
            if (!String.IsNullOrEmpty(user))
            {

                string sql = "SELECT UserName, UserRole = (SELECT Name FROM AspNetRoles r WHERE r.Id = ur.RoleId) FROM AspNetUsers u" +
	                         " LEFT JOIN AspNetUserRoles ur ON ur.UserId = u.Id "+
	                         " WHERE UserName = '" + user + "' ";

                //var Role = db.Database.SqlQuery<cUserRole>(sql).FirstOrDefault();
                 cUserRole Role = db.cUserRoles.FromSqlRaw(sql).First();
                 return Role.UserRole;
                //return "NA";
            }

            return "NA";
        }

        #endregion


        #region UserActivities

        //GET : get user activities by the user 
        public List<cUserActivity> GetUserActivities(string user, string sDate, string eDate)
        {
            try
            {

                if (!String.IsNullOrEmpty(eDate))
                {
                    var tempDate = DateTime.Parse(eDate);
                    eDate = tempDate.AddDays(1).ToShortDateString();
                }


                //eDate = DateTime.Parse(eDate).AddDays(1).ToShortDateString();
                List<cUserActivity> activity = new List<cUserActivity>();
                string dateQuery = "";
                if (sDate != "" && eDate != "")
                {
                    dateQuery = " AND (Date >= convert(datetime, '" + sDate + "') AND Date <= convert(datetime, '" + eDate + "'))  ";
                }

                //sql query with comma separated item list
                string sql =
                      "  SELECT Id , Date, Assigned, ProjectName, SalesCode, Amount, Remarks, Status,CustEntMainId,Type,ActivityType,CustEntActStatusId,CustEntActActionStatusId,CustEntActActionCodesId, "+
                      " Company = (SELECT Name FROM CustEntMains cem WHERE cem.Id = act.CustEntMainId ), "+
                      "     Points = ISNULL((SELECT Points FROM CustEntActivityTypes type WHERE type.Type = act.ActivityType), 0), " +
                      " SalesLeadId = ISNULL(SalesLeadId, 0 ), "+
                      " Discriminator = '', "+
                      " Commodity = ''  " +
                      " FROM CustEntActivities act WHERE " +
                      " Assigned = '" + user + "' " + dateQuery + " ";

                //TODO: fix query cUserActivity
                //activity = db.Database.SqlQuery<cUserActivity>(sql).ToList();
                //activity = db.cUserActivities.FromSqlRaw(sql).ToList();
                DateTime tempSDate = DateTime.Parse(sDate);
                DateTime tempEDate = DateTime.Parse(eDate);

                var CustActivities = db.CustEntActivities
                    .Include(c=>c.CustEntMain)
                    .Where(c => c.Assigned == user && c.Date >= tempSDate && c.Date <= tempEDate).ToList();

                tempEDate = DateTime.Parse(eDate);

                CustActivities.ForEach(c => {
                    cUserActivity tempAct = new cUserActivity();
                    tempAct.Id = c.Id;
                    tempAct.Date = c.Date;
                    tempAct.Assigned = c.Assigned;
                    tempAct.ProjectName = c.ProjectName;
                    tempAct.SalesCode = c.SalesCode;
                    tempAct.Amount = c.Amount ?? 0;
                    tempAct.Remarks = c.Remarks;
                    tempAct.CustEntMainId = c.CustEntMainId;
                    tempAct.Type = c.Type;
                    tempAct.ActivityType = c.ActivityType;
                    tempAct.CustEntActStatusId = c.CustEntActStatusId;
                    tempAct.CustEntActActionCodesId = c.CustEntActActionCodesId;
                    tempAct.Company = c.CustEntMain.Name;
                    tempAct.Points = GetActivityPoints(c.ActivityType);
                    tempAct.SalesLeadId = c.SalesLeadId;
                    tempAct.Status = c.Status;

                    activity.Add(tempAct);
                }) ;


                //Filter and Remove points on Duplicate Activity with the same code
                activity = FilterDuplicateActivity(activity);

                //Filter and Remove points on Quotation Activity
                activity = FilterQuotationActivity(activity);

                return activity;
            }
            catch (Exception ex)
            {
                throw ex;
                //return new List<cUserActivity>();
            }
        }

        private int GetActivityPoints(string type)
        {
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    var activity = db.CustEntActivityTypes.Where(c => c.Type == type).First();
                    return activity.Points;

                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
          
        }

        private List<cUserActivity> FilterDuplicateActivity(List<cUserActivity> activityList)
        {
            //holds the Ids of unique activity
            List<string> tempCodes = new List<string>();
            List<string> actTypes = new List<string>();

            foreach (var act in activityList)
            {
                var t1 = tempCodes.Contains(act.SalesCode);
                var t2 = actTypes.Contains(act.ActivityType);

                if (tempCodes.Contains(act.SalesCode) && actTypes.Contains(act.ActivityType))
                {
                    //If Id is in the list, remove the point
                    act.Points = 0;
                }
                else
                {
                  
                    //if Id is not in the list, add id to the list
                    //and retain the point
                    tempCodes.Add(act.SalesCode);
                    actTypes.Add(act.ActivityType);

                }
            }

            return activityList;
        }


        private List<cUserActivity> FilterQuotationActivity(List<cUserActivity> activityList)
        {
            //holds the Ids of unique activity
            List<string> tempCodes = new List<string>();

            foreach (var act in activityList)
            {
                if (act.ActivityType == "Quotation")
                {
                    if (act.Remarks == "Quotation Sent" || act.Remarks == "Awarded")
                    {
                        act.Points = 8;
                    }
                    else
                    {
                        act.Points = 0;
                    }
                }
            }

            return activityList;
        }


        //GET : return user performance report based on the count of each Activity Type
        public cUserPerformanceReport GetUserPerformance(List<cUserActivity> activities, string user)
        {
            cUserPerformanceReport performance = new cUserPerformanceReport();

            //get counts of each activity
            performance.User = UserRemoveEmail(user);
            performance.Sales = activities.Where(a => a.ActivityType == "Sales").Count();
            performance.Meeting = activities.Where(a => a.ActivityType == "Meeting").Count();
            performance.Quotation = activities.Where(a => a.ActivityType == "Quotation").Count();
            performance.Procurement = activities.Where(a => a.ActivityType == "Procurement").Count();
            performance.CallsAndEmail = activities.Where(a => a.ActivityType == "CallsAndEmail").Count();


            return performance;
        }

        //GET : return user performance report based on the total amount of each Activity Type
        public cUserSalesReport GetUserSales(List<cUserActivity> activities, string user)
        {
            cUserSalesReport sales = new cUserSalesReport();
            sales.TotalSales = 0;
            sales.TotalQuotation = 0;
            sales.TotalProcurement = 0;
            sales.TotalJobOrder = 0;
            //get total of each Activity Type
            foreach (var act in activities)
            {
                decimal tempAmt = act.Amount != null ? (decimal)act.Amount : 0;
                switch (act.ActivityType)
                {
                    case "Sales":
                        sales.TotalSales += tempAmt;
                        break;
                    case "Quotation":
                        sales.TotalQuotation += tempAmt;
                        break;
                    case "Procurement":
                        sales.TotalProcurement += tempAmt;
                        break;
                    case "Job Order":
                        sales.TotalJobOrder += tempAmt;
                        break;
                    default:
                        break;
                }
            }

            sales.User = UserRemoveEmail(user);


            return sales;
        }
        #endregion

        #region Supplier Activities
        //GET : get user activities by the user 
        public List<cUserActivity> GetSupActivities(string user, string sDate, string eDate)
        {
            //add 1 day
            if (!String.IsNullOrEmpty(eDate))
            {
                var tempDate = DateTime.Parse(eDate);
                eDate = tempDate.AddDays(1).ToShortDateString();
            }

            //eDate = DateTime.Parse(eDate).AddDays(1).ToShortDateString();
            List<cUserActivity> activity = new List<cUserActivity>();
            string dateQuery = "";
            if (sDate != "" && eDate != "")
            {
                dateQuery = " AND (DtActivity >= convert(datetime, '" + sDate + "') AND DtActivity <= convert(datetime, '" + eDate + "'))  ";
            }

            //sql query with comma separated item list
            string sql =
               @" SELECT *, Company = (SELECT Name FROM Suppliers sup WHERE sup.Id = act.SupplierId ), SupplierId as CustEntMainId,  
                  Points = (SELECT Points FROM SupplierActivityTypes type WHERE type.Type = act.ActivityType), 
                  Code as SalesCode ,DtActivity as Date
                  FROM SupplierActivities act WHERE " +
                  "Assigned = '" + user + "' " + dateQuery + " ORDER BY DtActivity DESC ;";
           
            //TODO: fix query cUserActivity
            //activity = db.Database.SqlQuery<cUserActivity>(sql).ToList();

            //Filter and Remove points on Duplicate Activity with the same code
            activity = FilterDuplicateActivity(activity);

            return activity;
        }


        //GET : return user performance report based on the count of each Activity Type
        public cUserPerformanceReport GetSupPerformance(List<cUserActivity> activities, string user)
        {
            cUserPerformanceReport performance = new cUserPerformanceReport();

            //get counts of each activity
            performance.User = UserRemoveEmail(user);
            performance.Close = activities.Where(a => a.ActivityType == "Close").Count();
            performance.Sales = activities.Where(a => a.ActivityType == "Job Order").Count();
            performance.Meeting = activities.Where(a => a.ActivityType == "Meeting").Count();
            performance.Procurement = activities.Where(a => a.ActivityType == "Procurement").Count();


            return performance;
        }
        #endregion

        #region Activity Post Sales 
        public List<cActivityPostSales> GetActivityPostSales(string status, string srch, string user, string role)
        {

            List<cActivityPostSales> activity = new List<cActivityPostSales>();

            //sql query with comma separated item list
            string sql =
               @" 
               SELECT *, cem.Name as Company FROM (
                    SELECT c.SalesCode,
                    ActivityDate = ( SELECT TOP 1 ca.Date FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    CompanyId    = ( SELECT TOP 1 ca.CustEntMainId FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    ProjectName  = ( SELECT TOP 1 ca.ProjectName FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    Status       = ( SELECT TOP 1 ca.Status FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    ActivityType = ( SELECT TOP 1 ca.ActivityType FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    Amount       = ( SELECT TOP 1 ca.Amount FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    AssignedTo   = ( SELECT TOP 1 ca.Assigned FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    Remarks      = ( SELECT TOP 1 ca.Remarks FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC )
                    from CustEntActivities c
                    Group by c.SalesCode 
                ) as act

                LEFT JOIN CustEntMains cem ON cem.Id = act.CompanyId

                WHERE convert(datetime, GETDATE()) >= CASE WHEN
                    act.ActivityType = 'Quotation' THEN
                      DATEADD(DAY, ISNULL(7, 0), act.ActivityDate) 
                    ELSE 
                      DATEADD(DAY, ISNULL(92, 0), act.ActivityDate) 
                    END 
                ";

            if (String.IsNullOrWhiteSpace(status))
            {
                sql += " AND act.Status != 'Close'";
            }
            else
            {
                sql += " AND act.Status = '" + status + "'";
            }

            if (role != "Admin")
            {
                sql += " AND (cem.Exclusive = 'PUBLIC' OR ISNULL(cem.Exclusive,'PUBLIC') = 'PUBLIC') OR (cem.Exclusive = 'EXCLUSIVE' AND cem.AssignedTo = '" + user + "') ";
            }

            if (!String.IsNullOrWhiteSpace(srch))
            {
                sql += " AND ( act.SalesCode Like '%" + srch + "%' OR cem.Name LIKE '%" + srch + "%' OR act.ProjectName LIKE '%" + srch + "%' ) ";
            }


            sql += " ORDER BY CASE WHEN act.ActivityType = 'Quotation' then 1 else 2 end, act.ActivityType DESC, act.ActivityDate DESC";

            //TODO: fix query cUserActivity
            //activity = db.Database.SqlQuery<cActivityPostSales>(sql).ToList();

            return activity;
        }


        #endregion


        #region Activity Status
        public List<cActivityActiveList> GetActiveActivities(string status, string user, string role)
        {

            List<cActivityActiveList> activity = new List<cActivityActiveList>();

            //sql query with comma separated item list
            string sql =
               @" 
               SELECT *, cem.Name as Company FROM (
                    SELECT c.SalesCode,
                    ActivityDate = ( SELECT TOP 1 ca.Date FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    CompanyId    = ( SELECT TOP 1 ca.CustEntMainId FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    ProjectName  = ( SELECT TOP 1 ca.ProjectName FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    Status       = ( SELECT TOP 1 ca.Status FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    ActivityType = ( SELECT TOP 1 ca.ActivityType FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
                    Amount       = ( SELECT TOP 1 ca.Amount FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC )
                    from CustEntActivities c
                    Group by c.SalesCode 
                ) as act

                LEFT JOIN CustEntMains cem ON cem.Id = act.CompanyId

                WHERE 
                ";

            if (String.IsNullOrWhiteSpace(status))
            {
                sql += " act.Status != 'Close'";
            }
            else
            {
                sql += " act.Status = '" + status + "'";
            }

            if (role != "Admin")
            {
                sql += " AND ( (cem.Exclusive = 'PUBLIC' OR ISNULL(cem.Exclusive,'PUBLIC') = 'PUBLIC') OR (cem.Exclusive = 'EXCLUSIVE' AND cem.AssignedTo = '" + user + "') )";
            }

            sql += " ORDER BY act.ActivityDate DESC";

            //TODO: fix query cUserActivity
            //activity = db.Database.SqlQuery<cActivityActiveList>(sql).ToList();

            return activity;
        }


        public List<cSupActivityActiveList> GetSupActiveActivities(string status, string user, string role)
        {

            List<cSupActivityActiveList> activity = new List<cSupActivityActiveList>();

            //sql query with comma separated item list
            string sql =
               @" 
               SELECT act.*, sup.Name FROM (
                    SELECT s.Code, 
                        DtActivity = ( SELECT TOP 1 su.DtActivity FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ),
                        SupId = ( SELECT TOP 1 su.SupplierId FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ),
                        StatusId = ( SELECT TOP 1 su.SupplierActStatusId FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ),
                        Activity = ( SELECT TOP 1 su.ActivityType FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ) ,
                        ActType = ( SELECT TOP 1 su.Type FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ) ,
                        Amount = ( SELECT TOP 1 ISNULL(su.Amount,0) FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ) 
                        FROM SupplierActivities s 
                        GROUP BY s.Code
                    ) as act 
                    LEFT JOIN Suppliers sup ON sup.Id = act.SupId
                    WHERE 
                ";

            if (String.IsNullOrWhiteSpace(status))
            {
                sql += " act.StatusId < 3 ";
            }
            else
            {
                sql += " act.StatusId = '" + status + "'";
            }

            sql += " ORDER BY act.DtActivity DESC";

            //TODO: fix query cUserActivity
            //activity = db.Database.SqlQuery<cSupActivityActiveList>(sql).ToList();

            return activity;
        }


        public string UserRemoveEmail(string input)
        {
            try
            {
                char ch = '@';
                int idx = input.IndexOf(ch);
                return input.Substring(0, idx);

            }
            catch
            {
                return input;
            }

        }
        #endregion
    }
}