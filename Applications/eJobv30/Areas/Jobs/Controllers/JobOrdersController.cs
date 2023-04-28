using eJobv30.Areas.Suppliers.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.DTO.ItemSchedule;
using RealSys.CoreLib.Models.DTO.Jobs;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using RealSys.Modules.Jobs;
using RealSys.Modules.SalesLeadLib.Lib;
using RealSys.Modules.SysLib.Lib;
using System.Diagnostics;
using System.Net;
//using System.Web.Mvc;
//using System.Web.Mvc;

namespace eJobv30.Areas.Jobs.Controllers
{
    [Area("Jobs")]
    public class JobOrdersController : Controller
    {   

         
        private ErpDbContext db;
        private DBClasses dbclasses;
        private JobOrderClass jobOrderServices;
        private JobVehicleClass jobVehicleServices;
        private DateClass date;
        private UserServices userServices;
        private readonly UserManager<IdentityUser> userManager;

        // NEW CUSTOMER Reference ID
        private int NewCustSysId = 1;

        // Job Status
        private int JOBINQUIRY = 1;
        private int JOBRESERVATION = 2;
        private int JOBCONFIRMED = 3;
        private int JOBCLOSED = 4;
        private int JOBCANCELLED = 5;


        private string SITECONFIG = "Realwheels";

        public JobOrdersController(ErpDbContext _context, SysDBContext _sysDBContext, ILogger<SuppliersController> _logger, UserManager<IdentityUser> _userManager)
        {
            db = _context;
            dbclasses = new DBClasses(_context, _sysDBContext, _logger);
            jobOrderServices = new JobOrderClass(_context, _logger);
            date = new DateClass();
            userServices = new UserServices(_context, _sysDBContext, _logger, _userManager);
            userManager = _userManager;
            jobVehicleServices = new JobVehicleClass(_context, _logger);
        }

        // GET: JobOrder
        public ActionResult Index(int? sortid, int? serviceId, int? mainid, string search)
        {
            #region Session
            //if (sortid != null)
            //    Session["FilterID"] = (int)sortid;
            //else
            //{
            //    if (Session["FilterID"] != null)
            //        sortid = (int)Session["FilterID"];
            //    else
            //        sortid = 1;
            //}

            //if (Session["FilterID"] == null)
            //{
            //    Session["FilterID"] = 1;
            //}
            if (sortid == null)
            {
                sortid = 1;
            }
            #endregion

            // get job list data
            var data = jobOrderServices.GetJobData((int)sortid);

            // Search Filter
            //if (search != "")
            //{
            //    data = jobOrderServices.GetSearchJobData(search);
            //}

            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;

            ViewBag.SrchValue = search;
            ViewBag.mainId = jobmainId;
            ViewBag.SiteConfig = SITECONFIG;
            ViewBag.companyList = db.Customers.ToList();
            ViewBag.JobVehicle = jobVehicleServices.GetJobVehicle(jobmainId);
            ViewBag.IsAdmin = User.IsInRole("Admin") || User.IsInRole("Accounting") ? true : false;

            if (sortid == 1)
            {
                return View(data.OrderBy(d => d.Main.JobDate));
            }
            else
            {
                return View(data.OrderByDescending(d => d.Main.JobDate));
            }
        }


        // GET: JobOrder/JobListing 
        // List of jobs by date with minimal information
        public ActionResult JobListing(int? sortid, int? serviceId, int? mainid, string search)
        {

            if (sortid != null)
                //Session["FilterID"] = (int)sortid;
                sortid = sortid;
            else
            {
                //if (Session["FilterID"] != null)
                //    sortid = (int)Session["FilterID"];
                //else
                    sortid = 1;
            }

            DateTime today = date.GetCurrentDate();

            var data = new List<cJobOrder>();

            //get date fom SQL query
            var confirmed = jobOrderServices.getJobConfirmedListing((int)sortid).Select(s => s.Id);

            IEnumerable<JobMain> jobMains = db.JobMains.Where(j => confirmed.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                .Include(j => j.JobEntMains)
                ;

            List<cjobCounter> jobActionCntr = jobOrderServices.GetJobActionCount(jobMains.Select(d => d.Id).ToList());

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.Expenses = 0;
                joTmp.DtStart = jobOrderServices.GetMinMaxServiceDate(main.Id, "min");
                joTmp.DtEnd = jobOrderServices.GetMinMaxServiceDate(main.Id, "max");

                List<JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;

                    joTmp.Main.AgreedAmt += svc.ActualAmt != null ? svc.ActualAmt : 0;
                    joTmp.Company = jobOrderServices.GetJobCompany(main.Id);
                    joTmp.Expenses += jobOrderServices.GetJobExpensesBySVC(svc.Id);

                    joTmp.Services.Add(cjoTmp);
                }

                cjobIncome cIncome = new cjobIncome();
                cIncome.Car = 0;
                cIncome.Tour = 0;
                cIncome.Others = 0;

                joTmp.isPosted = jobOrderServices.GetJobPostedInReceivables(joTmp.Main.Id);
                joTmp.PostedIncome = cIncome;
                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();
                joTmp.Main.JobDate = jobOrderServices.TempJobDate(joTmp.Main.Id);

                //job payments
                joTmp.Payment = jobOrderServices.GetjobPaymentTotal(main.Id);
                
                //add discounts
                //subtract discount amount
                joTmp.Main.AgreedAmt += jobOrderServices.GetJobDiscountAmount(main.Id);

                data.Add(joTmp);

            }

            switch (sortid)
            {
                case 1: //OnGoing
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBINQUIRY || d.Main.JobStatusId == JOBRESERVATION || d.Main.JobStatusId == JOBCONFIRMED)).ToList()
                        .Where(d => DateTime.Compare(d.Main.JobDate.Date.AddDays(1), today.Date) >= 0).ToList();
                    break;
                case 2: //prev
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBINQUIRY || d.Main.JobStatusId == JOBRESERVATION || d.Main.JobStatusId == JOBCONFIRMED)).ToList()
                        .Where(p => DateTime.Compare(p.Main.JobDate.Date, today.Date) < 0).ToList();

                    //Closed and Current Month List
                    var currentMonthIds = jobOrderServices.currentJobsMonth().Select(s => s.Id);   //get list if job ids of current month fom SQL query
                    var currentMonthJobs = jobOrderServices.GetJobListing(currentMonthIds);
                    ViewBag.CurrentMonth = currentMonthJobs;

                    //Old Open jobs
                    var olderJobsIds = jobOrderServices.olderOpenJobs().Select(s => s.Id);  //get list of older jobs that are not closed
                    var OldJobs = jobOrderServices.GetJobListing(olderJobsIds).Where(s => s.DtStart < today);
                    ViewBag.olderOpenJobs = OldJobs;

                    break;
                case 3: //close
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBCLOSED || d.Main.JobStatusId == JOBCANCELLED)).ToList()
                        .Where(p => p.Main.JobDate.AddDays(60).Date > today.Date).ToList();
                    break;

                default:
                    data = (List<cJobOrder>)data.ToList();
                    break;
            }

            ViewBag.companyList = jobOrderServices.GetCompanyList();
            ViewBag.mainId = jobOrderServices.GetJobMainIdByService(serviceId,mainid);
            ViewBag.today = today;

            if (sortid == 1)
            {
                return View(data.OrderBy(d => d.Main.JobDate));
            }
            else
            {
                return View(data.OrderByDescending(d => d.Main.JobDate));

            }
        }




        // GET: JobMains/jobCreate
        [Authorize]
        public ActionResult jobCreate(int? id)
        {
            DateClass today = new DateClass();

            JobMain job = new JobMain();
            job.JobDate = date.GetCurrentDate();
            job.DueDate = null;
            job.NoOfDays = 1;
            job.NoOfPax = 1;
            job.AgreedAmt = 0;

            if (SITECONFIG == "AutoCare")
            {
                job.JobRemarks = " ";
                job.Description = "GMS AutoCare";
            }

            if (id == null)
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", NewCustSysId);
            }
            else
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", id);
            }
            var signedUser = HttpContext.User.Identity.Name;
            ViewBag.CompanyList = db.CustEntMains.OrderBy(s => s.Name).ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").OrderBy(s => s.Name).ToList() ?? new List<Customer>();
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", 2);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", JOBCONFIRMED);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc");
            ViewBag.AssignedTo = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", signedUser);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", 2);
            ViewBag.SiteConfig = SITECONFIG;

            return View(job);
        }


        // POST: JobMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult jobCreate([Bind("Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber,AssignedTo,DueDate")] JobMain jobMain, int? CompanyId, int? JobPaymentStatusId)
        {

            if (ModelState.IsValid)
            {
                if (JobCreateValidation(jobMain))
                {
                    if (jobMain.Customer == null)
                    {
                        var customerRecord = db.Customers.Find(jobMain.CustomerId);
                        jobMain.Customer = customerRecord;
                    }

                    db.JobMains.Add(jobMain);
                    db.SaveChanges();

                    if (CompanyId != null)
                    {
                        //new company
                        AddjobCompany(jobMain.Id, (int)CompanyId);
                    }

                    if (JobPaymentStatusId != null)
                    {
                        AddJobPaymentStatus((int)JobPaymentStatusId, jobMain.Id);
                    }

                  //  dbc.addEncoderRecord("joborder", jobMain.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job");


                    if (jobMain.Customer.Name == "<< New Customer >>")
                    {
                        //Create New Customer Account
                        return RedirectToAction("CreateCustomer", new { CreateCustJobId = jobMain.Id });
                    }

                    return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobMain.Id });

                }
            }

            if (JobPaymentStatusId == null)
            {
                JobPaymentStatusId = 2;
            }


            if (jobMain.Id == 0)
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", NewCustSysId);
            }
            else
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.Id);
            }

            ViewBag.CompanyList = db.CustEntMains.ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status != "INC"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.AssignedTo = new SelectList(userServices.getUsers(), "UserName", "UserName", jobMain.AssignedTo);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", (int)JobPaymentStatusId);
            ViewBag.SiteConfig = SITECONFIG;

            return View(jobMain);
        }

        [Authorize]
        public ActionResult JobDetails(int jobid)
        {
            var jobMain = db.JobMains.Find(jobid);
            var companyId = db.JobEntMains.Where(s => s.JobMainId == jobMain.Id).FirstOrDefault() != null ?
                db.JobEntMains.Where(s => s.JobMainId == jobMain.Id).FirstOrDefault().CustEntMainId : 1;

            ViewBag.mainid = jobid;
            ViewBag.CompanyList = db.CustEntMains.ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", companyId);
            ViewBag.AssignedTo = new SelectList(userServices.getUsers(), "UserName", "UserName", jobMain.AssignedTo);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", jobOrderServices.GetLastJobPaymentStatusId((int)jobid));
            ViewBag.SiteConfig = SITECONFIG;

            return View(jobMain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobDetails([Bind("Id,JobDate,CompanyId,CustomerId,Description,NoOfPax,NoOfDays,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber,AssignedTo,DueDate")] JobMain jobMain,
            int? CompanyId, decimal? AgreedAmt, int? JobPaymentStatusId)
        {
            if (ModelState.IsValid)
            {
                if (JobCreateValidation(jobMain))
                {
                    if (jobMain.CustContactEmail == null && jobMain.CustContactNumber == null)
                    {
                        var cust = db.Customers.Find(jobMain.CustomerId);
                        jobMain.CustContactEmail = cust.Email;
                        jobMain.CustContactNumber = cust.Contact1;
                    }

                    //Console.WriteLine("AgreedAmt: "+AgreedAmt);
                    System.Diagnostics.Debug.WriteLine("AgreedAmt job: " + jobMain.AgreedAmt);
                    System.Diagnostics.Debug.WriteLine("AgreedAmt: " + AgreedAmt);

                    jobMain.AgreedAmt = AgreedAmt;
                    db.Entry(jobMain).State = EntityState.Modified;
                    db.SaveChanges();

                    System.Diagnostics.Debug.WriteLine("----");
                    System.Diagnostics.Debug.WriteLine("AgreedAmt job: " + jobMain.AgreedAmt);
                    System.Diagnostics.Debug.WriteLine("AgreedAmt: " + AgreedAmt);
                    jobOrderServices.EditjobCompany(jobMain.Id, (int)CompanyId);

                    //Edit job payment status
                    if (JobPaymentStatusId != null)
                    {
                        jobOrderServices.EditJobPaymentStatus((int)JobPaymentStatusId, jobMain.Id);
                    }

                    //job trail
                    //trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Edit Saved", jobMain.Id.ToString());


                    return RedirectToAction("JobServices", new { JobMainId = jobMain.Id });

                }
            }


            ViewBag.mainid = jobMain.Id;
            ViewBag.CompanyList = db.CustEntMains.ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status != "INC"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", CompanyId);
            ViewBag.AssignedTo = new SelectList(userServices.getUsers(), "UserName", "UserName", jobMain.AssignedTo);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", (int)JobPaymentStatusId);
            ViewBag.SiteConfig = SITECONFIG;

            return View(jobMain);
        }

        #region JobServices

        //[Authorize(Roles = "Admin,ServiceAdvisor")]
        public ActionResult JobServices(int? JobMainId, int? serviceId, int? sortid, string action)
        {

            if (sortid != null)
                //Session["FilterID"] = (int)sortid;
                sortid = sortid;
            else
            {
                //if (Session["FilterID"] != null)
                //    sortid = (int)Session["FilterID"];
                //else
                sortid = 1;
            }

            if (JobMainId == null)
            {
                //return new StatusCodeResult((int)StatusCode.BadRequest);
            }

            var Job = db.JobMains
                .Include(j => j.JobStatus)
                .Include(j => j.JobPickups)
                .Include(j => j.JobNotes)
                .Include(j => j.JobItineraries)
                .Include(j => j.SalesLeadLinks)
                .Include(j => j.Branch)
                .Include(j => j.CashExpenses)
                .Include(j => j.Customer)
                .Include(j => j.JobEntMains)
                .Include(j => j.JobMainPaymentStatus)
                .Include(j => j.JobPayments)
                .Include(j => j.JobPosts)
                .Include(j => j.JobSuppliers)
                .Include(j => j.JobTypes)
                .Include(j => j.SalesLeadLinks)
                .Where(d => d.Id == JobMainId).FirstOrDefault();

            var jobServiceItems = db.JobServices
                .Include(j => j.JobMain)
                .Include(j => j.Supplier)
                .Include(j => j.Service)
                .Include(j => j.SupplierItem)
                .Include(j => j.JobServiceItems)
                    .ThenInclude(j => j.InvItem)
                .Include(j => j.PickupInstructions)
                .Include(j => j.JobServicePickups)
                .Where(d => d.JobMainId == JobMainId);

            System.Collections.ArrayList providers = new System.Collections.ArrayList();

            foreach (var item in jobServiceItems)
            {
                if (item.Supplier != null)
                {
                    string sTmp = "";
                    try
                    {
                        sTmp = item.Supplier.Name;
                    }
                    catch
                    {
                        sTmp = "Pickup Details / Provider not defined.";
                    }

                    if (!providers.Contains(sTmp))
                    {
                        providers.Add(sTmp);
                    }
                }
            }

            var jobTrailsEncoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == JobMainId.ToString());

            if (jobTrailsEncoder.FirstOrDefault() != null)
            {
                ViewBag.JobEncoder = jobTrailsEncoder.FirstOrDefault();
            }
            else
            {
                ViewBag.JobEncoder = new JobTrail { Id = 0, Action = "Create", user = "none", dtTrail = DateTime.Now, RefId = "0", RefTable = "none" };
            }


            var isAllowedPayment = false;
            if (User.IsInRole("Admin") || User.IsInRole("Accounting"))
            {
                isAllowedPayment = true;
            }


            //check previlages
            var isAdmin = User.IsInRole("Admin");
            var isServiceAdvisor = User.IsInRole("ServiceAdvisor");

            ViewBag.IsAdmin = isAllowedPayment;
            ViewBag.IsAllowedEdit = isAdmin || isServiceAdvisor ? true : false;
            ViewBag.isOwner = User.IsInRole("Owner");
            ViewBag.JobMainId = (int)JobMainId;
            ViewBag.JobOrder = Job;
            ViewBag.Company = jobOrderServices.GetJobCompany((int)JobMainId);
            ViewBag.Providers = providers;
            ViewBag.JobStatus = Job.JobStatus.Status;
            ViewBag.JobStatusId = Job.JobStatusId;
            ViewBag.Itineraries = db.JobItineraries.Where(d => d.JobMainId == JobMainId).ToList();
            ViewBag.sortid = sortid;
            ViewBag.jobAction = action;
            ViewBag.user = HttpContext.User.Identity.Name;
            ViewBag.Vehicles = jobVehicleServices.GetCustomerVehicleList((int)JobMainId);
            ViewBag.JobVehicle = jobVehicleServices.GetJobVehicle((int)JobMainId);
            ViewBag.PaymentStatus = jobOrderServices.GetJobPaymentStatus((int)JobMainId);
            ViewBag.SiteConfig = SITECONFIG;
            ViewBag.IsJobPosted = jobOrderServices.GetJobPostedInReceivables((int)JobMainId);

            var veh = jobVehicleServices.GetCustomerVehicleList((int)JobMainId);
            return View(jobServiceItems.OrderBy(d => d.DtStart).ToList());

        }

        [Authorize]
        public ActionResult JobServiceAdd(int? JobMainId)
        {
            JobMain job = db.JobMains.Find((int)JobMainId);
            JobServices js = new JobServices();
            js.JobMainId = (int)JobMainId;

            DateTime dtTmp = new DateTime(job.JobDate.Year, job.JobDate.Month, job.JobDate.Day, 8, 0, 0);
            js.DtStart = dtTmp;
            js.DtEnd = dtTmp.AddDays(job.NoOfDays - 1).AddHours(10);
            //js.Remarks = "10hrs use per day P300/hr in excess, Driver and Fuel Included";
            js.Remarks = "10hrs use per day P350/hr in excess, Driver Included. Fuel by Renter.";
            js.ActualAmt = 0;
            js.QuotedAmt = 0;
            js.SupplierAmt = 0;

            var siteConfig = SITECONFIG;
            if (siteConfig == "AutoCare")
            {
                js.Remarks = " ";
            }

            //modify SupplierItem
            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();

            ViewBag.id = JobMainId;
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", job.Description);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name");
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description");
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name");
            return View(js);
        }


        // POST: JobServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult JobServiceAdd([Bind("Id,JobMainId,ServicesId,SupplierId,DtStart,DtEnd,Particulars,QuotedAmt,SupplierAmt,ActualAmt,Remarks,SupplierItemId")] JobServices jobServices)
        {
            if (ModelState.IsValid)
            {
                if (jobServices.QuotedAmt == null)
                {
                    jobServices.QuotedAmt = 0;
                    jobServices.ActualAmt = 0;
                }
                else
                {

                    jobServices.ActualAmt = jobServices.QuotedAmt;
                }

                jobServices.DtEnd = ((DateTime)jobServices.DtEnd).Add(new TimeSpan(23, 59, 59));
                db.JobServices.Add(jobServices);
                db.SaveChanges();

                try
                {
                    //set initial unit as unassigned
                    int UnassignedId = db.InvItems.Where(u => u.Description == "UnAssigned").FirstOrDefault().Id;
                   jobOrderServices.AddUnassignedItem(UnassignedId, jobServices.Id);
                }
                catch
                { }
            }

            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();

            ViewBag.id = jobServices.JobMainId;
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobServices.JobMainId);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name", jobServices.SupplierId);
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description", jobServices.SupplierItemId);
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name", jobServices.ServicesId);

            // dbc.addEncoderRecord("jobOrder/jobservice", jobServices.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job Service");

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobServices.JobMainId });
        }


        // GET: JobServices/Edit/5
        [Authorize]
        public ActionResult JobServiceEdit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            JobServices jobServices = db.JobServices.Find(id);
            if (jobServices == null)
            {
                return NotFound();
            }

            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();

            ViewBag.svcId = jobServices.Id;
            ViewBag.Sdate = jobServices.DtStart.ToString();
            ViewBag.Edate = jobServices.DtEnd.ToString();
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobServices.JobMainId);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name", jobServices.SupplierId);
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name", jobServices.ServicesId);
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description", jobServices.SupplierItemId);
            return View(jobServices);
        }

        // POST: JobServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobServiceEdit([Bind("Id,JobMainId,ServicesId,SupplierId,DtStart,DtEnd,Particulars,QuotedAmt,SupplierAmt,ActualAmt,Remarks,SupplierItemId")] JobServices jobServices)
        {
            if (ModelState.IsValid)
            {
                //jobServices.DtEnd = ((DateTime)jobServices.DtEnd).Add(new TimeSpan(23, 59, 59));
                db.Entry(jobServices).State = EntityState.Modified;

                DateTime dtSvc = (DateTime)jobServices.DtStart;
                List<JobItinerary> iti = db.JobItineraries.Where(d => d.JobMainId == jobServices.JobMainId && d.SvcId == jobServices.Id).ToList();
                foreach (var ititmp in iti)
                {
                    int iHr = dtSvc.Hour, iMn = dtSvc.Minute;
                    if (ititmp.ItiDate != null)
                    {
                        DateTime dtIti = (DateTime)ititmp.ItiDate;
                        iHr = dtIti.Hour;
                        iMn = dtIti.Minute;
                    }
                    ititmp.ItiDate = new DateTime(dtSvc.Year, dtSvc.Month, dtSvc.Day, iHr, iMn, 0);
                    db.Entry(ititmp).State = EntityState.Modified;
                }

                if (jobServices.QuotedAmt == null)
                {
                    jobServices.QuotedAmt = 0;
                    jobServices.ActualAmt = 0;
                }
                else
                {
                    jobServices.ActualAmt = jobServices.QuotedAmt;
                }


                //db.SaveChanges();
                jobOrderServices.UpdateJobDate(jobServices.JobMainId);
                db.SaveChanges();

                //job trail
                //trail.recordTrail("JobOrder/JobServiceEdit", HttpContext.User.Identity.Name, "JobService Edit Saved", jobServices.Id.ToString());


            }

            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();


            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobServices.JobMainId);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name", jobServices.SupplierId);
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name", jobServices.ServicesId);
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description", jobServices.SupplierItemId);

            //dbc.addEncoderRecord("jobOrder/jobservice", jobServices.Id.ToString(), HttpContext.User.Identity.Name, "Edit Job Service");


            return RedirectToAction("JobServices", "JobOrders", new { JobMainId = jobServices.JobMainId });

        }


        public ActionResult JobSvcDelete(int? id)
        {

            JobServices jobServices = db.JobServices.Find(id);
            int jId = jobServices.JobMainId;

            //remove jobservice pickup on job service pickups
            JobServicePickup jobpickup = db.JobServicePickups.Where(j => j.JobServicesId == id).FirstOrDefault();

            if (jobpickup != null)
            {
                db.JobServicePickups.Remove(jobpickup);
                db.SaveChanges();
            }


            //remove jobservice items
            var jobitems = db.JobServiceItems.Where(i => i.JobServicesId == id).ToList();
            if (jobitems != null)
            {
                db.JobServiceItems.RemoveRange(jobitems);
                db.SaveChanges();
            }

            db.JobServices.Remove(jobServices);
            db.SaveChanges();

            return RedirectToAction("JobServices", "JobOrders", new { JobMainId = jobServices.JobMainId });
        }


        public bool ConfirmJobSvcDelete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }


                JobServices jobServices = db.JobServices.Find(id);

                if (jobServices == null)
                {
                    return false;
                }

                int jId = jobServices.JobMainId;

                //remove jobservice pickup on job service pickups
                JobServicePickup jobpickup = db.JobServicePickups.Where(j => j.JobServicesId == id).FirstOrDefault();

                if (jobpickup != null)
                {
                    db.JobServicePickups.Remove(jobpickup);
                    db.SaveChanges();
                }


                //remove jobservice items
                var jobitems = db.JobServiceItems.Where(i => i.JobServicesId == id).ToList();
                if (jobitems != null)
                {
                    db.JobServiceItems.RemoveRange(jobitems);
                    db.SaveChanges();
                }

                db.JobServices.Remove(jobServices);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }



        public ActionResult BrowseInvItem_withScheduleJS(int JobServiceId)
        {
            getItemSchedReturn gret = dbclasses.ItemSchedules();
            var mainId = db.JobServices.Find(JobServiceId).JobMainId;
            ViewBag.mainId = mainId;
            ViewBag.dtLabel = gret.dLabel;
            ViewBag.serviceId = JobServiceId;
            ViewBag.JobMainId = mainId;
            return View(gret.ItemSched);
        }


        public bool JobCreateValidation(JobMain jobMain)
        {
            bool isValid = true;

            if (jobMain.JobDate == null)
            {
                ModelState.AddModelError("JobDate", "Invalid JobDate");
                isValid = false;
            }

            if (jobMain.Description == "")
            {
                ModelState.AddModelError("Description", "Invalid Description");
                isValid = false;
            }


            if (jobMain.CustContactNumber == "")
            {
                ModelState.AddModelError("CustContactNumber", "Invalid Contact Number");
                isValid = false;
            }
            else
            {
                if (jobMain.CustContactNumber.Length < 5)
                {
                    ModelState.AddModelError("CustContactNumber", "Invalid Contact Number");
                    isValid = false;
                }

            }
            return isValid;
        }

        #endregion

        #region JobClass Services

        public void AddjobCompany(int jobId, int companyId)
        {
            JobEntMain jobCompany = new JobEntMain();
            jobCompany.JobMainId = jobId;
            jobCompany.CustEntMainId = companyId;

            db.JobEntMains.Add(jobCompany);
            db.SaveChanges();
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


        #endregion

        #region JobStatus Action
        public ActionResult CloseJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 4;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            //trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CONFIRMED", id.ToString());

            //var postSaleRecord = CreateJobPostSalesRecord((int)id);
            return RedirectToAction("JobServices", "JobOrders", new { JobMainId = id });
        }

        public ActionResult CloseJobStatusFromList(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 4;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            //trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CONFIRMED", id.ToString());

            //var postSaleRecord = CreateJobPostSalesRecord((int)id);

            return RedirectToAction("Index", "JobOrders", new { mainid = id });
        }

        public ActionResult ConfirmJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 3;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            //trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CONFIRMED", id.ToString());

            return RedirectToAction("JobServices", "JobOrders", new { JobMainId = id });
        }

        public ActionResult CancelJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 5;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            //trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CANCELLED", id.ToString());

            return RedirectToAction("JobServices", "JobOrders", new { JobMainId = id });
        }


        #endregion


        #region JobServiceItems

        public void AddUnassignedItem(int itemId, int serviceId)
        {
            //string sqlstr = "Insert Into JobServiceItems([JobServicesId],[InvItemId]) values(" + serviceId.ToString() + "," + itemId.ToString() + ")";
            //db.JobServiceItems.FromSqlRaw(sqlstr);

            JobServiceItem jsItem = new JobServiceItem();
            jsItem.JobServicesId = serviceId;
            jsItem.InvItemId = itemId;

            db.JobServiceItems.Add(jsItem);
            db.SaveChanges();


        }

        public void RemoveUnassignedItem(int itemId, int serviceId)
        {
            //string sqlstr = "Delete from JobServiceItems where JobServicesId = " + serviceId.ToString()
            //    + " AND InvItemId = " + itemId.ToString();

            //db.JobServiceItems.FromSqlRaw(sqlstr);

            JobServiceItem jsItem = db.JobServiceItems.Where(s=>s.InvItemId == itemId && s.JobServicesId == serviceId).FirstOrDefault();

            db.JobServiceItems.Remove(jsItem);
            db.SaveChanges();


        }

        public ActionResult AddItem(int itemId, int serviceId)
        {
            //string sqlstr = "Insert Into JobServiceItems([JobServicesId],[InvItemId]) values(" + serviceId.ToString() + "," + itemId.ToString() + ")";
            //db.JobServiceItems.FromSqlRaw(sqlstr);



            JobServiceItem jsItem = new JobServiceItem();
            jsItem.JobServicesId = serviceId;
            jsItem.InvItemId = itemId;
            db.JobServiceItems.Add(jsItem);
            db.SaveChanges();

            //remove unassigned
            var TempUnassigned = db.InvItems.Where(s => s.Description == "UnAssigned").First().Id;
            //remove unassigned
            var jscount = db.JobServiceItems.Where(s => s.JobServicesId == serviceId && s.InvItemId == TempUnassigned).Count();

            if (jscount > 1)
            {
                var unassigned = TempUnassigned;
                RemoveUnassignedItem(unassigned, serviceId);

            }

            var mainId = db.JobServices.Find(serviceId).JobMainId;
            return RedirectToAction("Index", new { serviceId = serviceId });

        }

        public ActionResult JSAddItem(int itemId, int serviceId)
        {
            //string sqlstr = "Insert Into JobServiceItems([JobServicesId],[InvItemId]) values(" + serviceId.ToString() + "," + itemId.ToString() + ")";
            // db.JobServiceItems.FromSqlRaw(sqlstr);

            JobServiceItem jsItem = new JobServiceItem();
            jsItem.JobServicesId = serviceId;
            jsItem.InvItemId = itemId;

            db.JobServiceItems.Add(jsItem);
            db.SaveChanges();

            var TempUnassigned = db.InvItems.Where(s => s.Description == "UnAssigned").First().Id;
            //remove unassigned
            var jscount = db.JobServiceItems.Where(s => s.JobServicesId == serviceId && s.InvItemId == TempUnassigned).Count();

            if (jscount > 1)
            {
                    var unassigned = TempUnassigned;
                    RemoveUnassignedItem(unassigned, serviceId);
                
            }

            var itemName = db.InvItems.Find(itemId);
            var service = db.JobServices.Find(serviceId);

            //job trail
            //trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name,
            //    "Assign " + itemName.Description + " to jobID " + service.JobMainId + " ",
            //    serviceId.ToString());

            var mainId = db.JobServices.Find(serviceId).JobMainId;
            return RedirectToAction("JobServices", new { JobMainId = mainId });

        }

        public ActionResult RemoveItem(int itemId, int serviceId)
        {
            string sqlstr = "Delete from JobServiceItems where JobServicesId = " + serviceId.ToString()
                + " AND InvItemId = " + itemId.ToString();

            db.JobServiceItems.FromSqlRaw(sqlstr);

            var item = db.InvItems.Find(itemId);
            var job = db.JobServices.Find(serviceId).JobMain;

            //job trail
            //trail.recordTrail("Remove Item", HttpContext.User.Identity.Name, "Remove Item " + item.Description + " from " + job.Description, serviceId.ToString());

            return RedirectToAction("InventoryItemList", new { serviceId = serviceId });
        }

        public ActionResult JsRemoveItem(int itemId, int serviceId)
        {
            //string sqlstr = "Delete from JobServiceItems where JobServicesId = " + serviceId.ToString()
            //    + " AND InvItemId = " + itemId.ToString();

            //db.JobServiceItems.FromSqlRaw(sqlstr);


            JobServiceItem jsItem = db.JobServiceItems.Where(s => s.InvItemId == itemId && s.JobServicesId == serviceId).FirstOrDefault();

            db.JobServiceItems.Remove(jsItem);
            db.SaveChanges();


            var item = db.InvItems.Find(itemId);
            var job = db.JobServices.Find(serviceId).JobMain;

            //job trail
            //trail.recordTrail("Remove Item", HttpContext.User.Identity.Name, "Remove Item " + item.Description + " from " + job.Description, serviceId.ToString());

            var mainId = db.JobServices.Find(serviceId).JobMainId;
            return RedirectToAction("JobServices", new { JobMainId = mainId });
        }



        #endregion


        #region JobsAPI


        //GET : return customer email
        public string getCustomerEmail(int id)
        {
            string custEmail = db.Customers.Find(id).Email;
            return custEmail;
        }

        //GET : return customer contact number
        public string getCustomerNumber(int id)
        {
            string custNum2 = db.Customers.Find(id).Contact2;
            return custNum2;
        }

        //GET : return customer company name
        public string getCustomerCompany(int id)
        {
            var company = db.CustEntities.Where(s => s.CustomerId == id).FirstOrDefault();
            string companyNum = company != null ? company.CustEntMainId.ToString() : " ";
            return companyNum;
        }



        #endregion

    }
}
