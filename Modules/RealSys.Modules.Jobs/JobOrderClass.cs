using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.DTO.Jobs;
using RealSys.CoreLib.Models.Erp;
using RealSys.Modules.SysLib.Lib;

namespace RealSys.Modules.Jobs
{
    public class JobOrderClass
    {
        private ErpDbContext db;
        private DateClass dt;

        public JobOrderClass(ErpDbContext dbContext, ILogger logger)
        {
            db = dbContext;
            dt = new DateClass();
        }




        //GET : return list of jobs
        public List<cJobOrder> GetJobData(int sortid)
        {

            var confirmed = getJobConfirmedListing(sortid).Select(s => s.Id);

            IEnumerable<JobMain> jobMains = db.JobMains.Where(j => confirmed.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                ;

            var data = new List<cJobOrder>();

            var today = dt.GetCurrentDate();

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.Company = GetJobCompanyName(main.Id);
                joTmp.Customer = main.Customer.Name;
                joTmp.JobStatus = main.JobStatus.Status;


                List<JobServices> joSvc = db.JobServices
                    .Include(s => s.Service)
                    .Include(s => s.SupplierItem)
                    .Include(s => s.PickupInstructions)
                    .Include(s => s.Supplier)
                    .Include(s => s.SupplierItem)
                    .Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart)
                    .ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;
                    var ActionDone = db.JobActions.Where(d => d.JobServicesId == svc.Id).Select(s => s.SrvActionItemId);
                    cjoTmp.SvcActionsDone = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.SvcActions = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && !ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);
                    joTmp.Main.AgreedAmt += svc.ActualAmt;

                    joTmp.Services.Add(cjoTmp);


                }

                //get total amount from services
                joTmp.Amount = joTmp.Services.Select(c=>c.Service.ActualAmt ?? 0).Sum();

                //get min job date
                if (sortid == 1)
                {
                    joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);
                }
                else
                {
                    joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }

                joTmp.Payment += GetJobPaymentAmount(main.Id);

                data.Add(joTmp);
            }

            switch (sortid)
            {
                case 1: //OnGoing
                    data = (List<cJobOrder>)data
                       .Where(d => d.Main.JobDate.CompareTo(today.Date) >= 0).ToList();

                    break;
                case 2: //prev
                    data = (List<cJobOrder>)data
                        .Where(p => DateTime.Compare(p.Main.JobDate.Date, today.Date) < 0)
                        .ToList();
                    break;
                case 3: //close
                    data = (List<cJobOrder>)data
                        .Where(p => p.Main.JobDate.Date > today.Date.AddDays(-150)).ToList();
                    break;
                case 4: //cancelled
                    data = (List<cJobOrder>)data
                        .Where(p => p.Main.JobDate.Date > today.Date.AddDays(-150)).ToList();
                    break;

                default:
                    data = (List<cJobOrder>)data.ToList();
                    break;
            }

            return data;
        }

        //GET : return list of ONGOING jobs
        public List<cJobOrder> GetSearchJobData(string srch)
        {
            var searched = GetJobSearchQuery(srch).Select(s => s.Id);

            IEnumerable<JobMain> jobMains = db.JobMains.Where(j => searched.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                ;

            var data = new List<cJobOrder>();

            DateTime today = new DateTime();
            today = dt.GetCurrentDate();

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.Company = GetJobCompany(main.Id);


                List<JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;
                    var ActionDone = db.JobActions.Where(d => d.JobServicesId == svc.Id).Select(s => s.SrvActionItemId);
                    cjoTmp.SvcActionsDone = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.SvcActions = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && !ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);
                    joTmp.Main.AgreedAmt += svc.ActualAmt;

                    joTmp.Services.Add(cjoTmp);
                }

                //get min job date
                joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);

                joTmp.Payment += GetJobPaymentAmount(main.Id);

                data.Add(joTmp);
            }

            if (srch != null)
            {
                var srchData = data.Where(d => d.Main.Id.ToString() == srch ||
                                 d.Main.Description.ToLower().ToString().Contains(srch.ToLower()) ||
                                 d.Main.Customer.Name.ToLower().ToString().Contains(srch.ToLower()) ||
                                 d.Company.ToLower().ToString().Contains(srch.ToLower()));

                if (srchData != null)
                    data = srchData.ToList();
            }

            return data;
        }
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

            //TODO : fix get cJobConfirmed Items List
            //joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();
            joblist = db.cJobConfirmeds.FromSqlRaw(sql).ToList();

            if (joblist.Count() == 0)
            {
                var today = dt.GetCurrentDate(); ;

                var JobMainsId = db.JobMains.Where(j => j.JobDate >= today).Select(j => j.Id).Distinct().ToList();

                joblist = new List<cJobConfirmed>();
                JobMainsId.ForEach(c =>
                     joblist.Add(new cJobConfirmed
                     {
                         Id = c
                     })
                );
               
            }

            return joblist;

        }



        //GET jobcount 
        public List<cjobCounter> GetJobActionCount(List<Int32> jobidlist)
        {
            #region sqlstr
            string sqlstr = @"
                select max(x.jobid) JobId, x.Id CodeId, max(x.code) CodeDesc, sum(x.ActionCount) CntItem, sum(x.DoneCount) CntDone
                from 
                (

                select max(a.JobMainId) jobid , d.Id, max(d.CatCode) code, '0' as ActionCount, count(b.Id) DoneCount
                from JobServices a
                left outer join JobActions b on a.Id = b.JobServicesId
                left outer join SrvActionitems c on b.SrvActionItemId = c.Id
                left outer join SrvActionCodes d on c.SrvActionCodeId = d.Id
                Group by a.JobMainId,d.Id

                union

                select max(a.JobMainId) jobid , d.Id, max(d.CatCode) code, count(c.Id) as ActionCount, '0' as DoneCount
                from JobServices a
                left outer join [Services] b on a.ServicesId = b.Id
                left outer join SrvActionitems c on b.Id = c.ServicesId
                left outer join SrvActionCodes d on c.SrvActionCodeId = d.Id
                Group by a.JobMainId,d.Id
                )x Group by x.jobid, x.Id
                order by x.jobid
                ;

                ";
            #endregion


            //List<cjobCounter> jobcntr = db.Database.SqlQuery<cjobCounter>(sqlstr).Where(d => jobidlist.Contains(d.JobId)).ToList();
            List<cjobCounter> jobcntr = new List<cjobCounter>();

            return jobcntr;
        }


        //GET : the lastest date of the job based on the date today
        public DateTime GetMinMaxServiceDate(int mainId, string getType)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();

            List<DateTime> dateList = new List<DateTime>();

            //loop though all jobservices in the jobmain
            //to get the latest date
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart))
            {

                var svcDtStart = ((DateTime)svc.DtStart);
                var svcDtEnd = ((DateTime)svc.DtEnd);

                //add date to list
                dateList.Add(svcDtStart);
                dateList.Add(svcDtEnd);

            }

            //sort date ascending
            dateList = dateList.OrderBy(x => x.Date).ToList();

            //handle empty service date
            if (dateList.Count == 0)
            {
                dateList.Add(main.JobDate);
            }

            //return main.JobDate;
            if (getType.ToLower() == "min")
                return dateList.First();
            else if (getType.ToLower() == "max")
                return dateList.Last();
            else
                return dateList.Last();
        }


        //GET : get the date of the job based on the date today
        public DateTime TempJobDate(int mainId)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();
            DateTime minDate = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault().JobDate.Date;
            DateTime maxDate = new DateTime(1, 1, 1);
            DateTime today = new DateTime();

            today = dt.GetCurrentDate();

            //loop though all jobservices in the jobmain
            //to get the latest date
            var counter = 1;
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart))
            {
                var firstService = (DateTime)db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart).FirstOrDefault().DtStart;
                var svcDtStart = (DateTime)svc.DtStart;
                var svcDtEnd = (DateTime)svc.DtEnd;
                //get min date
                if (counter == 1)
                {
                    minDate = firstService;
                }

                // minDate >= (DateTime)svc.DtStart;
                if (DateTime.Compare(minDate, svcDtStart.Date) >= 0)
                {
                    minDate = svcDtStart.Date; //if minDate > Dtstart
                }

                if (DateTime.Compare(today, svcDtStart.Date) >= 0 && DateTime.Compare(today, svcDtEnd.Date) <= 0)
                {
                    minDate = today; //latest date is today or within the range of start date and end date
                    //skip
                }
                else
                {
                    if (DateTime.Compare(today, svcDtStart.Date) < 0 && DateTime.Compare(today, minDate) > 0)
                    {
                        minDate = svcDtStart.Date; //if Today < Dtstart but today is greater than smallest date
                    }
                }

                //get max date
                if (DateTime.Compare(maxDate, svcDtEnd.Date) <= 0)
                {
                    maxDate = svcDtEnd.Date;
                }
            }

            //today is equal to smallest start date
            if (DateTime.Compare(today, minDate) == 0)
            {
                main.JobDate = minDate;
            }

            //today is equal to highest end date
            if (DateTime.Compare(today, maxDate) == 0)
            {
                main.JobDate = maxDate;
            }

            //today is < smallest date
            if (DateTime.Compare(today, minDate) < 0)
            {
                main.JobDate = minDate;
            }

            //today is greater than the smallest date
            //job is currently on going, adjust date
            if (DateTime.Compare(today, minDate) > 0)
            {
                if (DateTime.Compare(today, maxDate) < 0)
                {
                    main.JobDate = today;
                }

                if (DateTime.Compare(today, maxDate) > 0)
                {
                    main.JobDate = minDate;
                }
            }

            if (minDate == new DateTime(9999, 12, 30))
            {
                main.JobDate = minDate;
            }

            return main.JobDate;
            //return minDate;
        }



        public decimal GetJobExpensesBySVC(int svcId)
        {
            decimal total = 0;
            var expense = db.JobExpenses.Where(s => s.JobServicesId == svcId).ToList();
            foreach (var items in expense as IEnumerable<JobExpenses>)
            {
                total += items.Amount;
            }
            return total;
        }


        //GET jobcount 
        public bool GetJobPostedInReceivables(int jobId)
        {
            #region sqlstr
            string sqlstr = "SELECT Id FROM ArTransactions ar WHERE ar.InvoiceId = " + jobId + ";";
            #endregion

            int count = 0;
            //int count = db.Database.SqlQuery<int>(sqlstr).Count();

            if (count > 0)
            {
                return true;
            }
            return false;
        }


        public decimal GetJobDiscountAmount(int jobId)
        {
            try
            {
                //get job discount payments of job
                var jobPaymentDiscounts = db.JobPayments.Where(p => p.JobMainId == jobId && p.JobPaymentTypeId == 4).ToList();
                if (jobPaymentDiscounts.Count != 0)
                {
                    decimal totalDiscount = 0;

                    //get total discount
                    foreach (var payment in jobPaymentDiscounts)
                    {
                        totalDiscount += payment.PaymentAmt;
                    }

                    return totalDiscount;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
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

           // joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;
        }

        //GET : return list of ONGOING jobs listing
        public IEnumerable<cJobOrder> GetJobListing(IEnumerable<int> joblist)
        {
            IEnumerable<JobMain> jobMains = db.JobMains.Where(j => joblist.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                .Include(j => j.JobEntMains)
                ;

            List<cjobCounter> jobActionCntr = GetJobActionCount(jobMains.Select(d => d.Id).ToList());
            var data = new List<cJobOrder>();

            DateTime today = dt.GetCurrentDate();

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.DtStart = GetMinMaxServiceDate(main.Id, "min");
                joTmp.DtEnd = GetMinMaxServiceDate(main.Id, "max");

                List<JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;

                    joTmp.Main.AgreedAmt += svc.ActualAmt != null ? svc.ActualAmt : 0;
                    joTmp.Company = db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault() != null ? db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault().CustEntMain.Name : "";

                    joTmp.Expenses += GetJobExpensesBySVC(svc.Id);

                    joTmp.Services.Add(cjoTmp);
                }

                cjobIncome cIncome = new cjobIncome();
                cIncome.Car = 0;
                cIncome.Tour = 0;
                cIncome.Others = 0;

                //var latestPosted = db.JobPosts.Where(j => j.JobMainId == main.Id).OrderByDescending(s => s.Id).FirstOrDefault();

                //if (latestPosted == null)
                //{
                //    joTmp.isPosted = false;
                //}
                //else
                //{
                //    cIncome.Car = latestPosted.CarRentalInc;
                //    cIncome.Tour = latestPosted.TourInc;
                //    cIncome.Others = latestPosted.OthersInc;
                //    joTmp.isPosted = true;
                //}

                joTmp.isPosted = GetJobPostedInReceivables(joTmp.Main.Id);

                joTmp.PostedIncome = cIncome;

                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();

                joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);

                List<JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }

                data.Add(joTmp);

            }
            return data.OrderBy(d => d.Main.JobDate).OrderByDescending(d => d.Main.JobDate);
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

            //joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;

        }
        public string GetJobCompany(int jobId)
        {
            var company = db.JobEntMains
                .Include(s=>s.CustEntMain)
                .Where(j => j.JobMainId == jobId);

            if (company.FirstOrDefault() != null)
            {
                return company.OrderByDescending(j => j.Id).FirstOrDefault().CustEntMain.Name;
            }

            //if no records
            return "Personal Account";
        }


        public JobPaymentStatus GetJobPaymentStatus(int id)
        {
            return db.JobPaymentStatus.Find(GetLastJobPaymentStatusId(id));

        }


        public int GetLastJobPaymentStatusId(int jobId)
        {
            var tempStatus = db.JobMainPaymentStatus.Where(j => j.JobMainId == jobId);

            if (tempStatus.FirstOrDefault() != null)
            {
                return tempStatus.OrderByDescending(j => j.Id).FirstOrDefault().JobPaymentStatusId;
            }

            //unpaid if no records
            return 2;
        }


        public void EditjobCompany(int jobId, int companyId)
        {
            //AddjobCompany(jobId, companyId);
            if (db.JobEntMains.Where(j => j.JobMainId == jobId).FirstOrDefault() == null)
            {
                //add if entry does not exist
                AddjobCompany(jobId, companyId);
            }
            else
            {
                //save changes if entry does not exist
                JobEntMain jobCompany = db.JobEntMains.Where(j => j.JobMainId == jobId).FirstOrDefault();
                jobCompany.JobMainId = jobId;
                jobCompany.CustEntMainId = companyId;

                db.Entry(jobCompany).State = EntityState.Modified;
                db.SaveChanges();
            }

        }

        public void AddjobCompany(int jobId, int companyId)
        {
            JobEntMain jobCompany = new JobEntMain();
            jobCompany.JobMainId = jobId;
            jobCompany.CustEntMainId = companyId;

            db.JobEntMains.Add(jobCompany);
            db.SaveChanges();
        }


        public bool EditJobPaymentStatus(int id, int jobId)
        {
            try
            {
                //get latest job payment status
                var tempPaymentStatus = db.JobMainPaymentStatus.Where(j => j.JobMainId == jobId);

                if (tempPaymentStatus.FirstOrDefault() == null)
                {
                    //add job payment status
                    AddJobPaymentStatus(id, jobId);
                }
                else
                {
                    //check if prev status is not current status
                    if (GetLastJobPaymentStatusId(jobId) != id)
                    {
                        //add job payment status
                        AddJobPaymentStatus(id, jobId);
                    }
                }


                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddJobPaymentStatus(int id, int jobId)
        {
            try
            {

                JobMainPaymentStatus paymentStatus = new JobMainPaymentStatus();
                paymentStatus.JobMainId = jobId;
                paymentStatus.JobPaymentStatusId = id;

                db.JobMainPaymentStatus.Add(paymentStatus);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public void UpdateJobDate(int mainId)
        {

            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();

            DateTime today = new DateTime();
            today = dt.GetCurrentDateTime().Date;

            //loop though all jobservices in the jobmain
            //to get the latest date
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == main.Id))
            {

                //get least latest date
                if (DateTime.Compare(today, (DateTime)svc.DtStart) >= 0)   //if today is later than datestart, assign datestart to jobdate, 
                {

                    if (DateTime.Compare(today, (DateTime)svc.DtEnd) <= 0) //if today earlier than date end, assign jobdate today
                    {
                        //assign date today
                        main.JobDate = DateTime.Today;
                        db.Entry(main).State = EntityState.Modified;
                    }
                    else
                    {
                        //assign latest basin on today
                        main.JobDate = (DateTime)svc.DtStart;
                        db.Entry(main).State = EntityState.Modified;
                    }

                }
                else  //if today is earlier than datestart, assign datestart to jobdate, 
                {
                    //main.JobDate = (DateTime)svc.DtStart;
                    //db.Entry(main).State = EntityState.Modified;
                }
                //db.SaveChanges();
            }
        }


        public void AddUnassignedItem(int itemId, int serviceId)
        {
            //string sqlstr = "Insert Into JobServiceItems([JobServicesId],[InvItemId]) values(" + serviceId.ToString() + "," + itemId.ToString() + ")";
            //db.Database.ExecuteSqlCommand(sqlstr);
            var mainId = db.JobServices.Find(serviceId).JobMainId;

            JobServiceItem jobServiceItem = new JobServiceItem();
            jobServiceItem.InvItemId = itemId;
            jobServiceItem.JobServicesId = serviceId;

            db.JobServiceItems.Add(jobServiceItem);
            db.SaveChanges();

        }


        //GET : return job company name
        public string GetJobCompanyName(int jobId)
        {
            var jobmain = db.JobMains.Find(jobId);

            var jobEntMainQuery = jobmain.JobEntMains.OrderByDescending(j => j.Id).FirstOrDefault();
            if (jobEntMainQuery != null)
            {
                return jobEntMainQuery.CustEntMain.Name;
            }

            return "N/A";
        }

        public int GetJobMainIdByService(int? serviceId, int? mainid)
        {
            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;

            return jobmainId;
        }

        public List<CustEntMain> GetCompanyList()
        {
            return db.CustEntMains.Where(c => c.Status == "ACT").ToList();
        }


        //GET : the lastest date of the job based on the date today
        public DateTime MinJobDate(int mainId)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();

            DateTime minDate = main.JobDate;
            DateTime maxDate = new DateTime(1, 1, 1);

            DateTime today = new DateTime();
            today = dt.GetCurrentDate();

            //loop though all jobservices in the jobmain
            //to get the latest date
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart))
            {

                var svcDtStart = (DateTime)svc.DtStart;
                var svcDtEnd = (DateTime)svc.DtEnd;
                //get min date

                // minDate >= (DateTime)svc.DtStart;
                if (DateTime.Compare(minDate, svcDtStart.Date) >= 0)
                {
                    minDate = svcDtStart.Date; //if minDate > Dtstart
                }

                if (DateTime.Compare(today, svcDtStart.Date) >= 0 && DateTime.Compare(today, svcDtEnd.Date) <= 0)
                {
                    minDate = today; //latest date is today or within the range of start date and end date
                    //skip
                }
                else
                {
                    if (DateTime.Compare(today, svcDtStart.Date) < 0 && DateTime.Compare(today, minDate) > 0)
                    {
                        minDate = svcDtStart.Date; //if Today < Dtstart but today is greater than smallest date
                    }
                }

                //get max date
                if (DateTime.Compare(maxDate, svcDtEnd.Date) <= 0)
                {
                    maxDate = svcDtEnd.Date;
                }
            }

            //return main.JobDate;
            return minDate;
        }


        public decimal GetJobPaymentAmount(int id)
        {
            try
            {
                var paymentList = db.JobPayments.Where(j => j.JobMainId == id && j.JobPaymentTypeId < 4).ToList();
                decimal totalPayment = 0;

                foreach (var payment in paymentList)
                {
                    totalPayment += payment.PaymentAmt;
                }


                //subtract discounted amount
                //note: discount amount is negative number
                totalPayment = totalPayment + GetJobDiscountAmount(id);

                return totalPayment;
            }
            catch
            {
                return 0;
            }

        }

        //Get Job Order Listing based on the sort
        public List<cJobConfirmed> GetJobSearchQuery(string srch)
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();
            string sql = "";
            int srchId = 0;

            //if (int.TryParse(srch.TrimStart('0'), out srchId))
            //{
            //    sql = @"select j.Id from JobMains j WHERE j.id = " + srchId + " ";
            //}
            //else
            //{
            //    sql = " select j.Id from JobMains j " +
            //           " LEFT JOIN Customers c ON j.CustomerId = c.Id " +
            //           " LEFT JOIN JobEntMains jem ON j.Id = jem.JobMainId " +
            //           " LEFT JOIN CustEntMains cem ON jem.CustEntMainId = cem.Id " +
            //           " WHERE cem.Name LIKE '%" + srch + "%' OR c.Name LIKE '%" + srch + "%' OR j.Description LIKE '%" + srch + "%' ";
            //}

            //terminator
            sql += ";";

            joblist = db.cJobConfirmeds.FromSqlRaw(sql).ToList();

            return joblist;

        }


        public decimal GetjobPaymentTotal(int jobmainId)
        {
            decimal PaymentTmp = 0;

            List<JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == jobmainId).ToList();
            foreach (var payment in jobPayment)
            {
                //add payments except discount (JobPaymentTypeId = 4)
                if (payment.JobPaymentTypeId != 4)
                {
                    PaymentTmp += payment.PaymentAmt;
                }
            }

            return PaymentTmp;
        }

    }
}