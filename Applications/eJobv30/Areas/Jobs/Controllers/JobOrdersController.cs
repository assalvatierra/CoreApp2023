using eJobv30.Areas.Suppliers.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.DTO.Jobs;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using RealSys.Modules.Jobs;
using RealSys.Modules.SalesLeadLib.Lib;
using RealSys.Modules.SysLib.Lib;

namespace eJobv30.Areas.Jobs.Controllers
{
    public class JobOrdersController : Controller
    {


        private ErpDbContext db;
        private DBClasses dbclasses;
        private JobOrderClass jobServices;
        private DateClass date;
        private UserServices userServices;
        private readonly UserManager<IdentityUser> userManager;

        // Job Status
        private int JOBINQUIRY = 1;
        private int JOBRESERVATION = 2;
        private int JOBCONFIRMED = 3;
        private int JOBCLOSED = 4;
        private int JOBCANCELLED = 5;

        public JobOrdersController(ErpDbContext _context, SysDBContext _sysDBContext, ILogger<SuppliersController> _logger, UserManager<IdentityUser> _userManager)
        {
            db = _context;
            dbclasses = new DBClasses(_context, _sysDBContext, _logger);
            jobServices = new JobOrderClass(_context, _logger);
            date = new DateClass();
            userServices = new UserServices(_context, _sysDBContext, _logger, _userManager);
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        // GET: JobOrder/JobListing 
        // List of jobs by date with minimal information
        public ActionResult JobListing(int? sortid, int? serviceId, int? mainid, string search)
        {

            //if (sortid != null)
            //    Session["FilterID"] = (int)sortid;
            //else
            //{
            //    if (Session["FilterID"] != null)
            //        sortid = (int)Session["FilterID"];
            //    else
            //        sortid = 1;
            //}

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

    }
}
