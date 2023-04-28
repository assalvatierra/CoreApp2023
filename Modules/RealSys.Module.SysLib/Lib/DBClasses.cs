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


        public getItemSchedReturn ItemSchedules()
        {
            #region get itemJobs
            string SqlStr = @"
                select  a.Id ItemId, c.JobMainId, c.Id ServiceId, c.Particulars, c.DtStart, c.DtEnd from 
                InvItems a
                left outer join JobServiceItems b on b.InvItemId = a.Id 
                left outer join JobServices c on b.JobServicesId = c.Id
                left outer join JobMains d on c.JobMainId = d.Id
                where d.JobStatusId < 4 AND c.DtStart >= DATEADD(DAY, -30, GETDATE())
                ;";
            //List<cItemSchedule> itemJobs = db.Database.SqlQuery<cItemSchedule>(SqlStr).ToList();
            List<cItemSchedule> itemJobs = new List<cItemSchedule>();
            //cItemSchedule
            #endregion

            List<ItemSchedule> ItemSched = new List<ItemSchedule>();
            int NoOfDays = 20;

            DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            dtStart = dtStart.AddDays(-1);

            ItemSched = GetUnitsSchedule(itemJobs);
            ItemSched.AddRange(GetOtherUnitsSchedule(itemJobs));

            //Day Label
            List<DayLabel> dLabel = new List<DayLabel>();
            for (int i = 0; i <= NoOfDays; i++)
            {
                DateTime dtDay = dtStart.AddDays(i);

                DayLabel dsTmp = new DayLabel();
                dsTmp.iDay = i + 1;
                dsTmp.sDayName = dtDay.ToString("ddd");
                dsTmp.sDayNo = dtDay.ToString("dd");

                dLabel.Add(dsTmp);
            }

            getItemSchedReturn dReturn = new getItemSchedReturn();
            dReturn.dLabel = dLabel;
            dReturn.ItemSched = ItemSched;

            return dReturn;
        }


        private List<ItemSchedule> GetUnitsSchedule(List<cItemSchedule> itemJobs)
        {

            int NoOfDays = 20;
            DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            List<ItemSchedule> ItemSched = new List<ItemSchedule>();

            dtStart = dtStart.AddDays(-1);

            var InvItems = db.InvItems.Where(s => s.OrderNo <= 200).ToList().OrderBy(s => s.OrderNo);
            var InvItemsOthers = db.InvItems.Where(s => s.OrderNo > 200).ToList().OrderBy(s => s.OrderNo);

            foreach (var tmpItem in InvItems)
            {
                ItemSchedule ItemTmp = new ItemSchedule();

                ItemTmp.ItemId = tmpItem.Id;
                ItemTmp.Item = tmpItem;
                ItemTmp.dayStatus = new List<DayStatus>();

                Console.WriteLine(ItemTmp.Item.Description);

                var JobServiceList = itemJobs.Where(d => d.ItemId == tmpItem.Id);
                for (int i = 0; i <= NoOfDays; i++)
                {
                    DayStatus dsTmp = new DayStatus();
                    dsTmp.Date = dtStart.AddDays(i);
                    dsTmp.Day = i + 1;
                    dsTmp.status = 0;
                    dsTmp.jobcount = 0;

                    //Check if your Messages collection exists
                    if (dsTmp.svc == null)
                    {
                        //It's null - create it
                        dsTmp.svc = new List<JobServices>();
                    }

                    foreach (var jsTmp in JobServiceList)
                    {
                        var jsStartDate = DateTime.Parse(jsTmp.DtStart.ToString()).Date;
                        var jsEndDate = DateTime.Parse(jsTmp.DtEnd.ToString()).Date;

                        int istart = dsTmp.Date.CompareTo(jsStartDate);
                        int iend = dsTmp.Date.CompareTo(jsEndDate);

                        if (istart >= 0 && iend <= 0)
                        {
                            var jobStatus = db.JobMains.Find(jsTmp.JobMainId).JobStatusId;

                            //3 == CONFIRMED
                            if (jobStatus == 3)
                            {
                                dsTmp.status += 1;
                            }

                            dsTmp.jobcount += 1;

                            JobServices js = db.JobServices.Where(j => j.Id == jsTmp.ServiceId).FirstOrDefault();
                            dsTmp.svc.Add(js);
                        }

                    }

                    ItemTmp.dayStatus.Add(dsTmp);
                }
                ItemSched.Add(ItemTmp);
            }

            return ItemSched;
        }



        private List<ItemSchedule> GetOtherUnitsSchedule(List<cItemSchedule> itemJobs)
        {

            int NoOfDays = 20;

            List<ItemSchedule> ItemSched = new List<ItemSchedule>();

            DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            dtStart = dtStart.AddDays(-1);

            var InvItemsOthers = db.InvItems.Where(s => s.OrderNo > 200).ToList().OrderBy(s => s.OrderNo);

            foreach (var tmpItem in InvItemsOthers)
            {
                var haveJob = false;

                ItemSchedule ItemTmp = new ItemSchedule();

                ItemTmp.ItemId = tmpItem.Id;
                ItemTmp.Item = tmpItem;
                ItemTmp.dayStatus = new List<DayStatus>();

                Console.WriteLine(ItemTmp.Item.Description);

                var JobServiceList = itemJobs.Where(d => d.ItemId == tmpItem.Id);
                for (int i = 0; i <= NoOfDays; i++)
                {
                    DayStatus dsTmp = new DayStatus();
                    dsTmp.Date = dtStart.AddDays(i);
                    dsTmp.Day = i + 1;
                    dsTmp.status = 0;
                    dsTmp.jobcount = 0;

                    //Check if your Messages collection exists
                    if (dsTmp.svc == null)
                    {
                        //It's null - create it
                        dsTmp.svc = new List<JobServices>();
                    }


                    foreach (var jsTmp in JobServiceList)
                    {
                        var jsStartDate = DateTime.Parse(jsTmp.DtStart.ToString()).Date;
                        var jsEndDate = DateTime.Parse(jsTmp.DtEnd.ToString()).Date;

                        int istart = dsTmp.Date.CompareTo(jsStartDate);
                        int iend = dsTmp.Date.CompareTo(jsEndDate);

                        if (istart >= 0 && iend <= 0)
                        {
                            haveJob = true;
                            var jobStatus = db.JobMains.Find(jsTmp.JobMainId).JobStatusId;

                            //3 == CONFIRMED
                            if (jobStatus == 3)
                            {
                                dsTmp.status += 1;
                            }

                            dsTmp.jobcount += 1;

                            JobServices js = db.JobServices.Where(j => j.Id == jsTmp.ServiceId).FirstOrDefault();
                            dsTmp.svc.Add(js);
                        }

                    }

                    ItemTmp.dayStatus.Add(dsTmp);

                }
                if (haveJob)
                {
                    ItemSched.Add(ItemTmp);
                }
            }

            return ItemSched;
        }


    }
}
