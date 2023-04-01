using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.DTO.Jobs;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.Modules.SysLib.Lib
{
    public class JobServices
    {

        ErpDbContext db;
        DateClass datetime;

        public JobServices(ErpDbContext _context, ILogger _logger)
        {
            db = _context;
            datetime = new DateClass();
        }


        //For JobListing
        //Get Jobs at the current month with status as CLOSED
        public List<cJobConfirmed> currentJobsMonth()
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            sql = "SELECT j.Id FROM JobMains j WHERE j.JobStatusId = 4 AND MONTH(j.JobDate) = MONTH(GETDATE()) AND YEAR(j.JobDate) = YEAR(GETDATE())";

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;
        }


        //For JobListing 
        //Get old Jobs with status as RESERVATION AND CONFIRMED NOT current month
        public List<cJobConfirmed> olderOpenJobs()
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            sql = "SELECT j.Id FROM JobMains j WHERE j.JobStatusId < 4 AND j.JobDate < GETDATE() AND MONTH(j.JobDate) != MONTH(GETDATE())";

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;

        }

        //For JobListing
        //Get Ongoing Jobs
        public List<cJobConfirmed> getJobConfirmedListing(int sortid)
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            //filter jobs based on statusId and date
            switch (sortid)
            {
                case 1: //OnGoing
                    sql = @"SELECT DISTINCT job.Id FROM ( 
                            SELECT jm.Id, jm.JobDate, jm.Description, jm.JobStatusId, js.DtStart, js.DtEnd, 
                            Customer = c.Name
                            FROM JobMains jm 
                            LEFT OUTER JOIN JobServices js ON jm.Id = js.JobMainId 
	                        LEFT OUTER JOIN Customers c ON jm.CustomerId = c.Id ) job 
                            WHERE job.DtStart >= convert(datetime,  CAST(GETUTCDATE() as date)) OR 
                                (job.DtStart <= convert(datetime,  CAST(GETUTCDATE() as date)) AND job.DtEnd >= convert(datetime,  CAST(GETUTCDATE() as date))) 
                            AND job.JobStatusId < 4 ";
                    break;
                case 2: //prev
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND MONTH(j.JobDate) = MONTH(GETDATE()) AND YEAR(j.JobDate) = YEAR(GETDATE()) ;";
                    break;
                case 3: //close
                    sql = "select j.Id from JobMains j where j.JobStatusId > 3 AND j.JobDate >= DATEADD(DAY, -120, GETDATE());";
                    break;
                default:
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -350, GETDATE());";
                    break;
            }

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;

        }

        public List<cJobConfirmed> getJobConfirmedList(int sortid)
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            //filter jobs based on statusId and date
            switch (sortid)
            {
                case 1: //OnGoing
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -60, GETDATE());";
                    break;
                case 2: //prev
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -60, GETDATE());";
                    break;
                case 3: //close
                    sql = "select j.Id from JobMains j where j.JobStatusId = 4 AND j.JobDate >= DATEADD(DAY, -120, GETDATE());";
                    break;
                case 4: //close
                    sql = "select j.Id from JobMains j where j.JobStatusId = 5 AND j.JobDate >= DATEADD(DAY, -120, GETDATE());";
                    break;
                default:
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -150, GETDATE());";
                    //jobMains = jobMains.ToList();
                    break;
            }

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;

        }

        //For Job Income Reporting 
        //Get all previous CLOSED jobs
        public List<cJobConfirmed> getAllClosedJobs(string sDate, string eDate, string type, string unit)
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            sql = "SELECT j.Id FROM JobMains j WHERE j.JobStatusId = 4 ";

            if (sDate != "")
            {
                sql += "AND j.JobDate >= '" + sDate + "'";
            }

            if (eDate != "")
            {
                sql += "AND j.JobDate <= '" + eDate + "'";
            }

            if (sDate == "" && eDate == "")
            {
                sql += " AND j.JobDate < GETDATE() AND j.JobDate >= DATEADD(DAY, -120, GETDATE())";
            }

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;

        }

        /*
         * Sql for getting the active jobs 
         * with date filter : ALL, TODAY , TOMMORROW and 2 Days after 
         * Sorted by Job service start date and pickup time 
         */
        public List<cActiveJobs> getActiveJobs(int FilterId)
        {
            List<cActiveJobs> joblist = new List<cActiveJobs>();
            string sql = "";

            sql = " SELECT js.Id,  js.JobMainId ,js.Particulars, JobName = j.Description , Service = ( SELECT s.Name FROM Services s WHERE js.ServicesId = s.Id )," +
                   " Customer = (SELECT c.Name FROM Customers c WHERE j.CustomerId = c.Id) , " +
                   " Company = (SELECT cem.Name FROM JobEntMains jem LEFT JOIN CustEntMains cem ON jem.CustEntMainId = cem.Id WHERE j.Id = jem.JobMainId ) , " +
                   " Item = (SELECT sup.Description FROM SupplierItems sup WHERE js.SupplierItemId = sup.Id )," +
                   " CONVERT(varchar, CAST( js.DtStart as DATETIME), 107) as DtStart," +
                   " CONVERT(varchar, CAST( js.DtEnd as DATETIME), 107) as DtEnd," +
                   " CONVERT(varchar, CAST(jp.JsDate as DATETIME),  107) as JsDate, " +
                   //" CAST( convert(varchar, isnull(jp.JsTime,'00:00:00'), 8) as TIME) as JsTime, jp.JsLocation, " +
                   " CONVERT(varchar, CAST( isnull(jp.JsTime, '00:00:00') as TIME),8) as JsTime, jp.JsLocation," +
                   " SORTDATE = CAST( DATEADD(hh, CAST(SUBSTRING( CAST( CAST( CONVERT(varchar, isnull(jp.JsTime,'00:00:00'), 8)  as TIME) as VARCHAR),1,2 ) as INT),DtStart) as DATETIME) " +
                   " FROM JobServices js" +
                   " LEFT JOIN JobMains j ON js.JobMainId = j.Id" +
                   " LEFT JOIN JobServicePickups jp ON jp.JobServicesId = js.Id ";

            switch (FilterId)
            {
                case 1: //all
                    sql += " WHERE j.JobStatusId < 4 ";
                    break;
                case 2://today
                    sql += " WHERE (js.DtStart = CAST(GETDATE()as DATE) ) AND j.JobStatusId < 4 ";
                    break;
                case 3://tommorrow
                    sql += " WHERE (js.DtStart > CAST(GETDATE()as DATE)  AND js.DtStart <=  DATEADD(DAY, 1, GETDATE())) AND j.JobStatusId < 4 ";
                    break;
                case 4: //2 days
                    sql += " WHERE (js.DtStart >= CAST(GETDATE()as DATE)  AND js.DtStart <=  DATEADD(DAY, 2, GETDATE())) AND j.JobStatusId < 4 ";
                    break;
                default:
                    sql += " WHERE (js.DtStart >= CAST(GETDATE()as DATE)  AND js.DtStart <=  DATEADD(DAY, 2, GETDATE())) AND j.JobStatusId < 4 ";
                    break;
            }

            sql += "";

            joblist = db.Database.SqlQuery<cActiveJobs>(sql).ToList();

            //assign item
            foreach (var item in joblist)
            {
                item.Assigned = db.JobServiceItems.Where(s => s.JobServicesId == item.Id).ToList();
            }

            return joblist;
        }

        public decimal getJobExpensesBySVC(int svcId)
        {
            decimal total = 0;
            var expense = db.JobExpenses.Where(s => s.JobServicesId == svcId).ToList();
            foreach (var items in expense as IEnumerable<JobExpenses>)
            {
                total += items.Amount;
            }
            return total;
        }


        public decimal getJobCollectible(int jobid)
        {
            decimal total = 0;
            decimal totalAmount = 0;
            decimal totalPayment = 0;

            var jobsvc = db.JobServices.Where(s => s.JobMainId == jobid).ToList();

            foreach (var svc in jobsvc)
            {

                totalAmount += svc.QuotedAmt != null ? (decimal)svc.QuotedAmt : 0;

            }
            var payments = db.JobPayments.Where(s => s.JobMainId == jobid).ToList();
            if (payments != null)
            {
                foreach (var pay in payments)
                {
                    totalPayment += pay.PaymentAmt;
                }
            }

            total = totalAmount - totalPayment;

            return total;
        }



        // ----- JobExpenses ------ //
        #region JobExpenses
        public decimal getJobPayment(int id)
        {
            var paymentList = db.JobPayments.Where(j => j.JobMainId == id).ToList();
            decimal totalPayment = 0;

            foreach (var payment in paymentList)
            {
                totalPayment += payment.PaymentAmt;
            }

            return totalPayment;
        }


        public decimal getJobCashPayment(int id)
        {
            var paymentList = db.JobPayments.Where(j => j.JobMainId == id && j.BankId == 1).ToList();
            decimal totalPayment = 0;

            foreach (var payment in paymentList)
            {
                totalPayment += payment.PaymentAmt;
            }

            return totalPayment;
        }


        public decimal getJobBankPayment(int id)
        {
            var paymentList = db.JobPayments.Where(j => j.JobMainId == id && j.BankId != 1).ToList();
            decimal totalPayment = 0;

            foreach (var payment in paymentList)
            {
                totalPayment += payment.PaymentAmt;
            }

            return totalPayment;
        }

        public decimal getExpenses(int jsid, int expensesId)
        {

            var driverExpenses = db.JobExpenses.Where(j => j.JobServicesId == jsid && j.ExpensesId == expensesId).ToList();
            decimal totalExpenses = 0;

            foreach (var expenses in driverExpenses)
            {
                totalExpenses += expenses.Amount;
            }

            return totalExpenses;
        }


        #endregion


    }
}
