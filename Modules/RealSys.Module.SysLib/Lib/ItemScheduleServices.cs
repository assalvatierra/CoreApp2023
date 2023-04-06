using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.DTO.ItemSchedule;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.Modules.SysLib.Lib
{
    public class ItemScheduleServices
    {

        ErpDbContext db;
        DateClass datetime;

        public ItemScheduleServices(ErpDbContext _context,  ILogger _logger)
        {
            db = _context;
            datetime = new DateClass();
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

            //TODO: Get list of Items
            List<cItemSchedule> itemJobs = new List<cItemSchedule>();


            //cItemSchedule
            #endregion

            List <ItemSchedule> ItemSched = new List<ItemSchedule>();
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

        public getItemSchedReturn ItemSchedulesByHour()
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
            //TODO: GET item schedule
            List<cItemSchedule> itemJobs = new List<cItemSchedule>();
            //cItemSchedule
            #endregion

            int NoOfDays = 24;
            DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            List<ItemSchedule> ItemSched = new List<ItemSchedule>();

            var InvItems = db.InvItems.Where(s => s.OrderNo <= 510).ToList().OrderBy(s => s.OrderNo);
            var ItemId = db.InvItems.Select(s => s.Id).ToList();


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
                    dsTmp.Hour = i + 1;
                    dsTmp.status = 0;

                    //Check if your Messages collection exists
                    if (dsTmp.svc == null)
                    {
                        //It's null - create it
                        dsTmp.svc = new List<JobServices>();
                    }


                    foreach (var jsTmp in JobServiceList)
                    {
                        int istart = dsTmp.Date.CompareTo(jsTmp.DtStart);
                        int iend = dsTmp.Date.CompareTo(jsTmp.DtEnd);

                        if (istart >= 0 && iend <= 0)
                        {
                            var jshourStart = DateTime.Parse(jsTmp.DtStart.ToString()).Hour;
                            var jshourEnd = DateTime.Parse(jsTmp.DtEnd.ToString()).Hour;
                            int ihourStart = TimeSpan.Compare(TimeSpan.FromHours(dsTmp.Hour), TimeSpan.FromHours(jshourStart));
                            int ihourEnd = TimeSpan.Compare(TimeSpan.FromHours(dsTmp.Hour), TimeSpan.FromHours(jshourStart));

                            if (ihourStart >= 0 && ihourEnd <= 0)
                            {
                                dsTmp.status += 1;
                                JobServices js = db.JobServices.Where(j => j.Id == jsTmp.ServiceId).FirstOrDefault();
                                dsTmp.svc.Add(js);
                            }
                        }

                    }

                    ItemTmp.dayStatus.Add(dsTmp);
                }


                ItemSched.Add(ItemTmp);
            }

            //Day Label
            List<DayLabel> dLabel = new List<DayLabel>();
            for (int i = 0; i <= NoOfDays; i++)
            {
                DateTime dtDay = dtStart.AddHours(i);

                DayLabel dsTmp = new DayLabel();
                dsTmp.iDay = i + 1;
                dsTmp.iHour = i + 1;
                dsTmp.sDayNo = dtDay.ToString("hh tt");
                dsTmp.sDayName = dtDay.ToString("MMM-d");

                dLabel.Add(dsTmp);
            }

            getItemSchedReturn dReturn = new getItemSchedReturn();
            dReturn.dLabel = dLabel;
            dReturn.ItemSched = ItemSched;

            return dReturn;
        }

    }
}
