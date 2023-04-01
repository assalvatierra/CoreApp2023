using RealSys.Modules.CustomersLib.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealSys.CoreLib.Models.Erp;
using Microsoft.EntityFrameworkCore;

namespace RealSys.Modules.CustomersLib.Lib
{
    public class JobVehicleClass
    {
        private ErpDbContext db;

        public JobVehicleClass(ErpDbContext dbContext)
        {
            db = dbContext;
        }


        public JobVehicle GetJobVehicle(int jobmainId)
        {
            return db.JobVehicles.Where(j => j.JobMainId == jobmainId).OrderByDescending(j => j.Id).FirstOrDefault() ?? null;
        }

        public IEnumerable<Vehicle> GetCustomerVehicleList(int jobmainId)
        {
            var job = db.JobMains.Find(jobmainId);
            if (job == null)
            {
                return null;
            }

            var customerId = job.CustomerId;

            var customerVehicles = db.Vehicles.Where(s => s.CustomerId == customerId).ToList();
            if (customerVehicles.Count() == 0)
            {
                return null;
            }

            return customerVehicles;
        }

        public bool AddJobVehicle(int? jobMainId, int? vehicleId, int? mileage)
        {

            try
            {

                JobVehicle jobVehicle = new JobVehicle()
                {
                    JobMainId = (int)jobMainId,
                    VehicleId = (int)vehicleId,
                    Mileage = (int)mileage
                };

                //save JobVehicle
                db.JobVehicles.Add(jobVehicle);


                //add vehicle to description
                var jobmain = db.JobMains.Find(jobMainId);
                var Vehicle = db.Vehicles.Find(vehicleId);

                jobmain.Description =
                    " " + Vehicle.VehicleModel.VehicleBrand.Brand +
                    " " + Vehicle.VehicleModel.Make +
                    " " + Vehicle.YearModel +
                    " (" + Vehicle.PlateNo + ")" + " Mileage: " + mileage.ToString();

                if (jobmain.Description.Length >= 80)
                {
                    jobmain.Description = jobmain.Description.Substring(0, 80);
                }

                db.Entry(jobmain).State = EntityState.Modified;


                db.SaveChanges();

                return true;

            }
            catch
            {

                return false;
            }
        }


        public List<JobVehicleService> GetJobVehicleServices(int vehicleId)
        {
            try
            {
                if (vehicleId == 0)
                {
                    return new List<JobVehicleService>();
                }
                string SqlStr =
                         " SELECT jv.*, DtStart = ISNULL(js.DtStart, jm.JobDate), js.Particulars, js.Remarks,  JobMainId = jm.Id, jm.JobStatusId, "
                       + " JsServices = (SELECT s.Name FROM Services s WHERE s.ID = js.ServicesId) "
                       + " FROM JobVehicles jv "
                       + " LEFT JOIN JobServices js ON js.JobMainId = jv.JobMainId"
                       + " LEFT JOIN JobMains jm ON jm.Id = jv.JobMainId "
                       + " WHERE jv.VehicleId = " + vehicleId + " AND jm.JobStatusId <= 4 ORDER BY DtStart DESC ;";

                return new List<JobVehicleService>();

                //var sqlvehicleServices = db.JobVehicleService.ExecuteSqlRaw<JobVehicleService>(SqlStr).ToList();


                var sqlCustomers = db.Customers.FromSqlRaw(SqlStr).ToList();

                //if (sqlvehicleServices != null)
                //{
                //    foreach (var vehicle in sqlvehicleServices)
                //    {
                //        custList.Add(new CustomerList
                //        {
                //            Id = cust.Id,
                //            Company = cust.CustEntities.First().Company,
                //            Contact1 = cust.Contact1,
                //            Contact2 = cust.Contact2,
                //            JobCount = cust.JobMains.Count,
                //            Name = cust.Name,
                //            Status = cust.Status
                //        });
                //    }
                //}


                //return vehicleServices;
            }
            catch
            {
                return new List<JobVehicleService>();
            }
        }

        public int GetVehiclePrevOdo(int vehicleId)
        {
            try
            {
                var vehicleOdoRecords = db.JobVehicles.Where(j => j.VehicleId == vehicleId).ToList();

                if (vehicleOdoRecords.Count() > 0)
                {
                    var lastRecord = vehicleOdoRecords.OrderByDescending(j => j.Id).FirstOrDefault();

                    return lastRecord.Mileage;
                }
                else
                {
                    //no prev records
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
    }

}
