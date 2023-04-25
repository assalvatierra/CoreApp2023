using eJobv30.Areas.Suppliers.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.DTO.Jobs;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using RealSys.Modules.Jobs;
using RealSys.Modules.SalesLeadLib.Lib;
using RealSys.Modules.SysLib.Lib;
using System.Net;
//using System.Web.Mvc;

namespace eJobv30.Areas.Jobs.Controllers
{
    [Area("Jobs")]
    public class JobOrdersController : Controller
    {   

         
        private ErpDbContext db;
        private DBClasses dbclasses;
        private JobOrderClass jobServices;
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
            jobServices = new JobOrderClass(_context, _logger);
            date = new DateClass();
            userServices = new UserServices(_context, _sysDBContext, _logger, _userManager);
            userManager = _userManager;
            jobVehicleServices = new JobVehicleClass(_context, _logger);
        }

        public IActionResult Index()
        {
            return View();
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

            var data = new List<cJobOrder>();

            //get date fom SQL query
            var confirmed = jobServices.getJobConfirmedListing((int)sortid).Select(s => s.Id);

            IEnumerable<JobMain> jobMains = db.JobMains.Where(j => confirmed.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                .Include(j => j.JobEntMains)
                ;
            List<cjobCounter> jobActionCntr = jobServices.GetJobActionCount(jobMains.Select(d => d.Id).ToList());

            DateTime today = date.GetCurrentDate();
            ViewBag.today = today;

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.Expenses = 0;
                joTmp.DtStart = jobServices.GetMinMaxServiceDate(main.Id, "min");
                joTmp.DtEnd = jobServices.GetMinMaxServiceDate(main.Id, "max");

                List<JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;

                    joTmp.Main.AgreedAmt += svc.ActualAmt != null ? svc.ActualAmt : 0;
                    joTmp.Company = db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault() != null ? db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault().CustEntMain.Name : "";
                    joTmp.Expenses += jobServices.GetJobExpensesBySVC(svc.Id);

                    joTmp.Services.Add(cjoTmp);

                    //calculate total rate and payment
                }

                cjobIncome cIncome = new cjobIncome();
                cIncome.Car = 0;
                cIncome.Tour = 0;
                cIncome.Others = 0;

                joTmp.isPosted = jobServices.GetJobPostedInReceivables(joTmp.Main.Id);
                joTmp.PostedIncome = cIncome;
                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();
                joTmp.Main.JobDate = jobServices.TempJobDate(joTmp.Main.Id);

                //job payments
                joTmp.Payment = 0;
                List<JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    //add payments except discount (JobPaymentTypeId = 4)
                    if (payment.JobPaymentTypeId != 4)
                    {
                        joTmp.Payment += payment.PaymentAmt;
                    }
                }

                //add discounts
                //subtract discount amount
                joTmp.Main.AgreedAmt += jobServices.GetJobDiscountAmount(main.Id);

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
                    var currentMonthIds = jobServices.currentJobsMonth().Select(s => s.Id);   //get list if job ids of current month fom SQL query
                    var currentMonthJobs = jobServices.GetJobListing(currentMonthIds);
                    ViewBag.CurrentMonth = currentMonthJobs;


                    //Old Open jobs
                    var olderJobsIds = jobServices.olderOpenJobs().Select(s => s.Id);  //get list of older jobs that are not closed
                    var OldJobs = jobServices.GetJobListing(olderJobsIds).Where(s => s.DtStart < today);
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

            List<Customer> customers = db.Customers.ToList();
            ViewBag.companyList = customers;

            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;
            ViewBag.mainId = jobmainId;

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
                .Include(j=>j.JobStatus)
                .Include(j=>j.JobPickups)
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

            var jobServiceItems = db.JobServices.Include(j => j.JobMain).Include(j => j.Supplier).Include(j => j.Service)
                .Include(j => j.SupplierItem).Include(j => j.JobServicePickups).Where(d => d.JobMainId == JobMainId);

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
            ViewBag.Company = jobServices.GetJobCompany((int)JobMainId);
            ViewBag.Providers = providers;
            ViewBag.JobStatus = Job.JobStatus.Status;
            ViewBag.JobStatusId = Job.JobStatusId;
            ViewBag.Itineraries = db.JobItineraries.Where(d => d.JobMainId == JobMainId).ToList();
            ViewBag.sortid = sortid;
            ViewBag.jobAction = action;
            ViewBag.user = HttpContext.User.Identity.Name;
            ViewBag.Vehicles = jobVehicleServices.GetCustomerVehicleList((int)JobMainId);
            ViewBag.JobVehicle = jobVehicleServices.GetJobVehicle((int)JobMainId);
            ViewBag.PaymentStatus = jobServices.GetJobPaymentStatus((int)JobMainId);
            ViewBag.SiteConfig = SITECONFIG;
            ViewBag.IsJobPosted = jobServices.GetJobPostedInReceivables((int)JobMainId);

            var veh = jobVehicleServices.GetCustomerVehicleList((int)JobMainId);
            return View(jobServiceItems.OrderBy(d => d.DtStart).ToList());

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


        #region JobServices

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
