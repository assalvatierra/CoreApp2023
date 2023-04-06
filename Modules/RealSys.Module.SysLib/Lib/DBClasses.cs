using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.DTO.Common;
using RealSys.CoreLib.Models.DTO.ItemSchedule;
using RealSys.CoreLib.Models.DTO.TripLogs;
using RealSys.CoreLib.Models.DTO.UnitPackages;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.Modules.SysLib.Lib
{
    public class DBClasses
    {

        ErpDbContext db;
        SysDBContext sdb;
        DateClass datetime;

        public DBClasses(ErpDbContext _context, SysDBContext _sysDBContext, ILogger _logger)
        {
            db = _context;
            sdb = _sysDBContext;
            datetime = new DateClass();
        }


        //record encoder info 
        public void addEncoderRecord(string reftable, string refid, string user, string action)
        {


            db.JobTrails.Add(new JobTrail
            {
                RefTable = reftable,
                RefId = refid,
                user = user,
                Action = action,
                dtTrail = datetime.GetCurrentDateTime(),
                IPAddress = GetIPAddress()

            });

            db.SaveChanges();
        }

        protected string GetIPAddress()
        {
            //System.Web.HttpContext context = System.Web.HttpContext.Current;
            //string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //if (!string.IsNullOrEmpty(ipAddress))
            //{
            //    string[] addresses = ipAddress.Split(',');
            //    if (addresses.Length != 0)
            //    {
            //        return addresses[0];
            //    }
            //}

            //return context.Request.ServerVariables["REMOTE_ADDR"];

            return "NA";
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

    }
}
