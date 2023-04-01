using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.Modules.SysLib.Lib
{
    public class NotificationServices
    {
        ErpDbContext db;
        SysDBContext sdb;
        DateClass datetime;

        public NotificationServices(ErpDbContext _context, ILogger _logger)
        {
            db = _context;
            datetime = new DateClass();
        }


        public void addNotification(string Module, string Desc)
        {

            db.JobNotificationRequests.Add(new JobNotificationRequest
            {
                ReqDt = DateTime.Parse(DateTime.Now.ToString("MMM dd yyyy HH:mm:ss")),
                ServiceId = 4   //SMS service Id
            });
            db.SaveChanges();


            //db.JobServices.Add(new JobServices
            //{
            //    Id = 0,
            //    SupplierId = 1,
            //    SupplierItemId = 1,
            //    JobMainId = 4,
            //    ServicesId = 1,
            //    Remarks = Module + " - " + Desc
            //});
            db.SaveChanges();
        }

        public void addTestNotification(int transId, string webhookId)
        {

            db.JobNotificationRequests.Add(new JobNotificationRequest
            {
                ReqDt = DateTime.Parse(DateTime.Now.ToString("MMM dd yyyy HH:mm:ss")),
                ServiceId = transId,   //SMS service Id
                RefId = webhookId.ToString()
            });
            db.SaveChanges();

        }

    }
}
