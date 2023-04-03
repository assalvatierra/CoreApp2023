using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.DTO.SalesLeads;
using RealSys.CoreLib.Models.DTO.TripLogs;
using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.Modules.SysLib.Lib
{
    public class TripServices
    {

        ErpDbContext db;
        DateClass datetime;

        public TripServices(ErpDbContext _context, ILogger _logger)
        {
            db = _context;
            datetime = new DateClass();
        }


        //------ Trip Listing ----- //
        #region TripListing
        public List<TripListing> GetTripList(int? DateRange, string srch)
        {
            if (DateRange == null)
            {
                DateRange = 7;
            }

            string SqlStr = @" 
                SELECT js.JobMainId, js.Id as JobServicesId, js.DtStart, js.DtEnd, js.Particulars, jm.Description, jm.JobStatusId, js.ActualAmt
                , items = SUBSTRING((SELECT item = (SELECT ii.Description FROM InvItems ii WHERE ii.Id = jsi.InvItemId ) FROM JobServiceItems jsi WHERE jsi.InvItemId = js.Id FOR XML PATH('')),2,100)
	            FROM JobServices  js 
	            LEFT JOIN JobMains jm ON jm.Id = js.JobMainId 
	            WHERE js.DtEnd >= DATEADD(DAY, -30, GETDATE()) AND JobStatusId > 1 AND JobStatusId < 4 
            ;";

            //List<cTripList> JobList = db.Database.SqlQuery<cTripList>(SqlStr).ToList();

            //TODO: get triplist
            List<cTripList> JobList = new List<cTripList>();


            List <TripListing> tripList = new List<TripListing>();

            //get jobs 5 days
            //get today
            var range = DateRange;
            var today = datetime.GetCurrentDate();
            var tempDate = today.AddDays(-(int)range);
            var prevId = 0;
            List<int> JobIdList = new List<int>();

            for (var i = 0; i <= range; i++)
            {
                var prevDate = tempDate;

                foreach (var trip in JobList)
                {
                    //check date
                    if (tempDate.CompareTo(trip.DtStart) >= 0 && tempDate.CompareTo(trip.DtEnd) <= 0 && CheckCarlist(trip.JobServicesId))
                    {
                        tripList.Add(new TripListing
                        {
                            Id = trip.Id,
                            JobMainId = trip.JobMainId,
                            JobServicesId = trip.JobServicesId,
                            DtService = tempDate,
                            DtStart = trip.DtStart,
                            DtEnd = trip.DtEnd,
                            Unit = getCar(trip.JobServicesId),
                            Driver = getDriver(trip.JobServicesId),
                            Particulars = trip.Particulars,
                            Description = trip.Description,
                            ActualAmt = trip.ActualAmt != null ? trip.ActualAmt : 0,
                            ItemCode = trip.ItemCode,
                            JobStatus = trip.JobStatusId.ToString(),
                            ViewLabel = trip.ViewLabel,
                            Fuel = GetJobExpenseByCategory(trip.JobServicesId, 1),
                            DriverComi = GetJobExpenseByCategory(trip.JobServicesId, 3),
                            OperatorComi = GetJobExpenseByCategory(trip.JobServicesId, 8),
                            items = trip.items,
                            Payment = getJobPayment(trip.JobMainId),
                            DriverForRelease = GetForReleaseStatus(trip.JobServicesId, 3),
                            DriverIsReleased = GetForReleaseStatus(trip.JobServicesId, 3),
                            OperatorForRelease = GetForReleaseStatus(trip.JobServicesId, 8),
                            OperatorIsReleased = GetForReleaseStatus(trip.JobServicesId, 8)
                        });

                        prevId = trip.JobServicesId;
                    }

                }

                tempDate = tempDate.AddDays(1);
            }

            //search string
            if (srch != null)
            {
                //get jobservice items ids
                var jsIds = db.JobServiceItems.Where(s => s.InvItem.Description.Contains(srch) || s.InvItem.ItemCode.Contains(srch)).ToList().Select(c => c.JobServicesId).ToList();

                //tripList = tripList.Where(s ).ToList();
                tripList = tripList.Where(s => jsIds.Contains(s.JobServicesId)).ToList();


            }

            return tripList.OrderBy(s => s.Unit.FirstOrDefault()).OrderByDescending(s => s.DtService).ToList();
        }

        // get unit list of string of the job 
        // PARAM : id = jobserviceId 
        private List<string> getCar(int id)
        {
            var carList = db.JobServiceItems.Where(s => s.JobServicesId == id && s.InvItem.ViewLabel.ToUpper() == "UNIT").ToList();

            List<string> units = new List<string>();

            foreach (var car in carList)
            {
                string item = car.InvItem.Description + " (" + car.InvItem.ItemCode + ")";
                units.Add(item);
            }

            return units;
        }

        private bool CheckCarlist(int id)
        {
            var carList = db.JobServiceItems.Where(s => s.JobServicesId == id && s.InvItem.ViewLabel.ToUpper() == "UNIT").ToList();

            if (carList.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // get driver list of string of the job 
        // PARAM : id = jobserviceId
        private List<string> getDriver(int id)
        {
            var driverList = db.JobServiceItems.Where(s => s.JobServicesId == id && s.InvItem.ViewLabel.ToUpper() == "DRIVER").ToList();

            List<string> driverString = new List<string>();

            foreach (var driver in driverList)
            {
                driverString.Add(driver.InvItem.ItemCode);
            }

            return driverString;
        }


        // Get the total driver comi expenses of the job 
        // PARAM : id = jobserviceId
        private decimal GetJobExpenseByCategory(int id, int category)
        {
            var Expenses = db.JobExpenses.Where(s => s.JobServicesId == id && s.ExpensesId == category).Select(s => s.Amount).ToList();
            decimal total = 0;

            foreach (var expense in Expenses)

            {
                total += expense;
            }

            return total;
        }

        private bool GetForReleaseStatus(int jsId, int statusId)
        {
            var released = false;
            try
            {
                released = (bool)db.JobExpenses.Where(s => s.JobServicesId == jsId && s.ExpensesId == statusId).FirstOrDefault().ForRelease;
            }
            catch
            { }

            return released;
        }

        private bool GetIsReleaseStatus(int jsId, int statusId)
        {
            var released = false;
            try
            {
                released = (bool)db.JobExpenses.Where(s => s.JobServicesId == jsId && s.ExpensesId == statusId).FirstOrDefault().IsReleased;
            }
            catch
            { }

            return released;
        }

        public List<cDriverTrip> GetDriversTrip(int id, string sDate, string eDate)
        {
            List<cDriverTrip> trip = new List<cDriverTrip>();


            if (id != 0 && sDate != null && eDate != null)
            {

                string SqlStr = @"
                        SELECT je.*, js.DtStart, js.DtEnd, jm.Description, js.Particulars, ii.Description as Name, ii.ItemCode  FROM JobExpenses je 
	                        LEFT JOIN JobMains jm ON jm.Id = je.JobMainId 
	                        LEFT JOIN JobServices js ON js.Id = je.JobServicesId 
	                        LEFT JOIN JobServiceItems jsi ON jsi.JobServicesId = js.Id 
	                        LEFT JOIN InvItems ii ON ii.Id = jsi.InvItemId ";

                SqlStr += "WHERE ii.Id = " + id + " AND ForRelease = 1 ;";

                //trip = db.Database.SqlQuery<cDriverTrip>(SqlStr).ToList();
            }
            return trip;
        }

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

        #endregion 
    }
}
