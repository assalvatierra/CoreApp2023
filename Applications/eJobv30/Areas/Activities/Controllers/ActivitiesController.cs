using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RealSys.CoreLib.Models.DTO.Activities;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using RealSys.Modules.ActivitiesLib;
using RealSys.Modules.SysLib.Lib;
using Reporting.Data.Services;
using System.Net;

namespace eJobv30.Areas.Activities.Controllers
{
    [Area("Activities")]
    public class ActivitiesController : Controller
    {
        private ActivitiesServices ac ;
        private ErpDbContext db ;
        private DateClass dt ;
        private DBClasses dbc ;
        private RealSys.Modules.SysLib.Lib.UserServices userServices;

        public ActivitiesController(ILogger<ActivitiesController> logger, ErpDbContext erpDb, SysDBContext sysDBContext, UserManager<IdentityUser> _userManager)
        {

            db = erpDb;
            userServices = new RealSys.Modules.SysLib.Lib.UserServices(erpDb, sysDBContext, logger, _userManager);
            dbc = new DBClasses(erpDb, sysDBContext, logger);
            ac = new ActivitiesServices(erpDb);
            dt = new DateClass();
        }

        private List<SelectListItem> ActivityStatus = new List<SelectListItem> {
                new SelectListItem { Value = "Others", Text = "Others" },
                new SelectListItem { Value = "Indicated Price", Text = "Indicated Price" },
                new SelectListItem { Value = "Bidding Only", Text = "Bidding Only" },
                new SelectListItem { Value = "Firm Inquiry", Text = "Firm Inquiry" },
                new SelectListItem { Value = "Buying Inquiry", Text = "Buying Inquiry" },
                new SelectListItem { Value = "Job Order", Text = "Job Order" }
                };

        private List<SelectListItem> ActivityType = new List<SelectListItem> {
                new SelectListItem { Value = "Meeting", Text = "Meeting" },
                new SelectListItem { Value = "Quotation", Text = "Quotation" },
                new SelectListItem { Value = "Sales", Text = "Sales" }
                };

        // GET: Activities
        [Authorize]
        public ActionResult Index(string sdate, string edate)
        {
            var user = HttpContext.User.Identity.Name;
            var isAdmin = false;
            var date1 = new DateTime();
            var date2 = new DateTime();

            //handle date
            if (sdate != null || edate != null)
            {
                date1 = DateTime.Parse(sdate);
                date2 = DateTime.Parse(edate).AddDays(1);
            }
            else
            {
                date1 = dt.GetCurrentDate().AddMonths(-1);
                date2 = dt.GetCurrentDate().AddDays(1);
            }

            //handle user roles
            if (User.IsInRole("Admin"))
            {
                isAdmin = true;
                ViewBag.IsAdmin = isAdmin;

                //get activities of all users on Supplier
                ViewBag.SupplierActivities = ac.GetSupplierActivitiesAdmin(date1, date2);

                //get activities of all users on Companies
                var companyActivity = ac.GetCompanyActivitiesAdmin(date1, date2);
                return View(companyActivity);
            }
            else
            {
                ViewBag.IsAdmin = isAdmin;

                //get activities of the user on Supplier
                ViewBag.SupplierActivities = ac.GetSupplierActivitiesUser(user, date1, date2);

                //get activities of the user on Companies
                var companyActivity = ac.GetCompanyActivitiesUser(user, date1, date2);
                return View(companyActivity);
            }
        }


        // GET: IndexActivity
        public ActionResult IndexActivity(string code)
        {
            if (code != null)
            {
                var custEntActivities = db.CustEntActivities.Where(c => c.SalesCode == code).Include(c => c.CustEntMain);
                var activities = custEntActivities.OrderByDescending(c => c.Date).ToList();

                List<CustEntActivity> GroupActivities = new List<CustEntActivity>();
                foreach (var act in activities.GroupBy(c => c.Date))
                {
                    CustEntActivity tempAct = act.FirstOrDefault();
                    string tempRemarks = "";
                    foreach (var groupedAct in act)
                    {
                        tempRemarks += groupedAct.Remarks + " " + Environment.NewLine;

                    }
                    tempAct.Remarks = tempRemarks;

                    GroupActivities.Add(tempAct);
                }

                ViewBag.code = code;

                return View(GroupActivities);
            }



            return View(new List<CustEntActivity>());
        }


        #region Performance 
        public ViewResult Performance(string sdate, string edate)
        {
            var user = HttpContext.User.Identity.Name;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            //var isAdmin = false;

            //handle date when null
            if (sdate != null && edate != null)
            {
                startDate = DateTime.Parse(sdate);
                endDate = DateTime.Parse(edate);
            }
            else
            {
                startDate = dt.GetCurrentDate().AddMonths(-1);
                endDate = dt.GetCurrentDate();
            }

            if (User.IsInRole("Admin"))
            {
                var userReport = ac.GetUserPerformanceReport(startDate, endDate);
                userReport = AssignUserRoles(userReport);
                return View(userReport);
            }
            else
            {
                if (user != null)
                {
                    var userReport = ac.GetUserPerformanceReport(user, startDate, endDate);
                    userReport = AssignUserRoles(userReport);
                    return View(userReport);
                }
              
            }

            return View(new cUserPerformance());
        }

        public List<cUserPerformance> AssignUserRoles(List<cUserPerformance> UserList)
        {
            foreach (var user in UserList)
            {
                user.Role = ac.GetUserRole(user.UserName);
            }

            return UserList;
        }

        #endregion

        #region Company Edit
        // GET: CustEntActivities/Edit/5
        public ActionResult CompanyActsEdit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            CustEntActivity custEntActivity = db.CustEntActivities.Find(id);
            if (custEntActivity == null)
            {
                return NotFound();
            }
            ViewBag.Assigned = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", custEntActivity.Status);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", custEntActivity.Type);
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", custEntActivity.ActivityType);
            ViewBag.Id = custEntActivity.CustEntMainId;
            return View(custEntActivity);
        }

        // POST: CustEntActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyActsEdit([Bind("Id,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,CustEntMainId,Type,ActivityType,Commodity")] CustEntActivity custEntActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custEntActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assigned = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", custEntActivity.Status);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", custEntActivity.Type);
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", custEntActivity.ActivityType);
            ViewBag.Id = custEntActivity.CustEntMainId;
            return RedirectToAction("Index", "CompanyActsEdit", new { id = custEntActivity.Id });
        }


        // GET: CustEntActivities/Delete/5
        public ActionResult CompanyActsDelete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            CustEntActivity custEntActivity = db.CustEntActivities.Find(id);
            if (custEntActivity == null)
            {
                return NotFound();
            }

            var companyId = custEntActivity.CustEntMainId;

            ViewBag.Id = companyId;

            return View(custEntActivity);
        }

        // POST: CustEntActivities/Delete/5
        [HttpPost, ActionName("CompanyActsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyActsDeleteConfirmed(int id)
        {
            CustEntActivity custEntActivity = db.CustEntActivities.Find(id);
            var companyId = custEntActivity.CustEntMainId;
            db.CustEntActivities.Remove(custEntActivity);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Supplier Activities

        // GET: SupplierActivities/Edit/5
        public ActionResult SupplierActsEdit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            if (supplierActivity == null)
            {
                return NotFound();
            }
            ViewBag.Assigned = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(ActivityType, "value", "text", supplierActivity.Type);
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type", supplierActivity.ActivityType);
            ViewBag.SupplierActStatusId = new SelectList(db.SupplierActStatus, "Id", "Status", supplierActivity.SupplierActStatusId);
            ViewBag.SupplierActActionCodeId = new SelectList(db.SupplierActActionCodes, "Id", "Name", supplierActivity.SupplierActActionCodeId);
            ViewBag.SupplierActActionStatusId = new SelectList(db.SupplierActActionStatus, "Id", "ActionStatus", supplierActivity.SupplierActActionStatusId);
            ViewBag.Currency = new SelectList(db.Currencies, "Name", "Name");

            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupplierActsEdit([Bind("Id,Code,DtActivity,Assigned,Remarks,SupplierId,Amount,Currency,Type,ProjName,ActivityType,SupplierActStatusId,SupplierActActionCodeId,SupplierActActionStatusId")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assigned = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(ActivityType, "value", "text", supplierActivity.Type);
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Id", "Type", supplierActivity.ActivityType);
            ViewBag.SupplierActStatusId = new SelectList(db.SupplierActStatus, "Id", "Status", supplierActivity.SupplierActStatusId);
            ViewBag.SupplierActActionCodeId = new SelectList(db.SupplierActActionCodes, "Id", "Name", supplierActivity.SupplierActActionCodeId);
            ViewBag.SupplierActActionStatusId = new SelectList(db.SupplierActActionStatus, "Id", "ActionStatus", supplierActivity.SupplierActActionStatusId);
            ViewBag.Currency = new SelectList(db.Currencies, "Name", "Name");
            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // GET: SupplierActivities/Delete/5
        public ActionResult SupplierActsDelete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            if (supplierActivity == null)
            {
                return NotFound();
            }
            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Delete/5
        [HttpPost, ActionName("SupplierActsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SupplierActsDeleteConfirmed(int id)
        {

            //find supplier sales lead link
            var salesLeadSupplierActs = db.SalesLeadSupActivities.Where(s => s.SupplierActivityId == id);
            if (salesLeadSupplierActs.FirstOrDefault() != null)
            {
                //remove link
                db.SalesLeadSupActivities.Remove(salesLeadSupplierActs.FirstOrDefault());
                db.SaveChanges();

            }

            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            db.SupplierActivities.Remove(supplierActivity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region User Activitiy

        //Get User Activities within a specific date range
        [Authorize]
        public ActionResult UserActivities(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {
                var custAct = ac.GetUserActivities(user, sDate, eDate);

                ViewBag.User = user;
                ViewBag.IsAdmin = User.IsInRole("Admin");
                ViewBag.PerfSummary = ac.GetUserPerformance(custAct, user);
                ViewBag.SalesSummary = ac.GetUserSales(custAct, user);

                //For Create Activity View for the user
                Partial_ActivityCreate(user);

                return View(custAct);
            }

            return RedirectToAction("Index");
        }

        // GET : User Activities report from date range
        [HttpGet]
        public string GetUserActivitySummary(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {
                List<cUserActivity> tempAct = new List<cUserActivity>();
                var custAct = ac.GetUserActivities(user, sDate, eDate);
                foreach (var act in custAct)
                {
                    act.Assigned = act.UserRemoveEmail(act.Assigned);
                    tempAct.Add(act);
                }

                //convert list to json object
                return JsonConvert.SerializeObject(tempAct, Formatting.Indented);
            }

            return "Error";
        }

        // GET : User Activity Type report from date range
        [HttpGet]
        public string GetUserTypeSummary(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {

                var custAct = ac.GetUserActivities(user, sDate, eDate);
                var userPerf = ac.GetUserPerformance(custAct, user);

                //convert list to json object
                return JsonConvert.SerializeObject(userPerf, Formatting.Indented);
            }

            return "Error";
        }

        // GET : User sales report from date range
        [HttpGet]
        public string GetUserSalesSummary(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {
                var custAct = ac.GetUserActivities(user, sDate, eDate);
                var userPerf = ac.GetUserSales(custAct, user);

                //convert list to json object
                return JsonConvert.SerializeObject(userPerf, Formatting.Indented);
            }

            return "Error";
        }

        public void Partial_ActivityCreate(string user)
        {
            ViewBag.Assigned = user;
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status");
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type");
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type");

            CustEntActivity activity = new CustEntActivity();
            activity.Amount = 0;
            activity.Date = dt.GetCurrentDateTime();
            activity.Assigned = user;
            ViewBag.CustEntActivity = activity;
        }

        //CREATE : Activities/CreateActivity
        [HttpPost]
        public string CreateActivity(string actDate, string Assigned, string ProjectName, string SalesCode, string Amount, string Status, string Remarks, string CustEntMainId, string Type, string ActivityType)
        {
            try
            {
                //create activity
                CustEntActivity act = new CustEntActivity();
                act.Date = DateTime.Parse(actDate);
                act.Assigned = Assigned;
                act.ProjectName = ProjectName;
                act.SalesCode = SalesCode;
                act.Amount = Decimal.Parse(Amount);
                act.Status = Status;
                act.Remarks = Remarks;
                act.CustEntMainId = int.Parse(CustEntMainId);
                act.Type = Type;
                act.ActivityType = ActivityType;

                db.CustEntActivities.Add(act);
                db.SaveChanges();

                return "New Activity Added";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //GET : Activities/GetActivityRecord
        [HttpGet]
        public string GetActivityRecord(int id)
        {
            try
            {
                //create activity
                CustEntActivity act = db.CustEntActivities.Find(id);
                if (act != null)
                {
                    cUserActivity userAct = new cUserActivity();
                    userAct.Id = act.Id;
                    userAct.Date = act.Date;
                    userAct.ProjectName = act.ProjectName;
                    userAct.SalesCode = act.SalesCode;
                    userAct.Amount = act.Amount;
                    userAct.Status = act.Status;
                    userAct.Remarks = act.Remarks;
                    userAct.Company = act.CustEntMain.Name;
                    userAct.CustEntMainId = act.CustEntMainId;
                    userAct.Assigned = act.Assigned;
                    userAct.Type = act.Type;
                    userAct.ActivityType = act.ActivityType;

                    return JsonConvert.SerializeObject(userAct, Formatting.Indented);
                }
                return "ERROR";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //EDIT : Activities/EditActivity
        [HttpPost]
        public string EditActivity(int id, string actDate, string Assigned, string ProjectName, string SalesCode, string Amount, string Status, string Remarks, string CustEntMainId, string Type, string ActivityType)
        {
            try
            {
                //create activity
                CustEntActivity act = db.CustEntActivities.Find(id);
                act.Date = DateTime.Parse(actDate);
                act.Assigned = Assigned;
                act.ProjectName = ProjectName;
                act.SalesCode = SalesCode;
                act.Amount = Decimal.Parse(Amount);
                act.Status = Status;
                act.Remarks = Remarks;
                act.CustEntMainId = int.Parse(CustEntMainId);
                act.Type = Type;
                act.ActivityType = ActivityType;

                db.Entry(act).State = EntityState.Modified;
                db.SaveChanges();

                return "Edit Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //DELETE : Activities/DeleteActivityRecord
        [HttpPost]
        public string DeleteActivityRecord(int id)
        {
            try
            {
                //create activity
                CustEntActivity act = db.CustEntActivities.Find(id);
                if (act != null)
                {

                    db.CustEntActivities.Remove(act);
                    db.SaveChanges();
                    return "DELETE SUCCESS ";
                }
                return "DELETE ERROR ";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }



        #endregion

        #region Supplier Activities

        //Get User Activities within a specific date range
        public ActionResult SupActivities(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {
                var supAct = ac.GetSupActivities(user, sDate, eDate);

                ViewBag.User = user;
                ViewBag.IsAdmin = User.IsInRole("Admin");
                ViewBag.PerfSummary = ac.GetSupPerformance(supAct, user);
                ViewBag.SalesSummary = ac.GetUserSales(supAct, user);

                //For Create Activity View for the user
                Partial_SupActivity(user);


                return View(supAct);
            }

            return RedirectToAction("Index");
        }

        public void Partial_SupActivity(string user)
        {
            ViewBag.Assigned = user;
            ViewBag.SupplierId = new SelectList(db.Suppliers.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type");
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type");

            SupplierActivity activity = new SupplierActivity();
            activity.DtActivity = dt.GetCurrentDateTime();
            activity.Assigned = user;
            ViewBag.supActivity = activity;
        }

        // GET : User Activities report from date range 
        [HttpGet]
        public string GetSupActivitySummary(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {
                List<cUserActivity> tempAct = new List<cUserActivity>();
                var custAct = ac.GetSupActivities(user, sDate, eDate);
                foreach (var act in custAct)
                {
                    act.Assigned = act.UserRemoveEmail(act.Assigned);
                    tempAct.Add(act);
                }

                //convert list to json object
                return JsonConvert.SerializeObject(tempAct, Formatting.Indented);
            }

            return "Error";
        }


        //CREATE : Activities/CreateActivity
        [HttpPost]
        public string CreateSupActivity(string actDate, string Assigned, string Code, string Amount, string Remarks, string SupplierId, string Type, string ActivityType)
        {
            try
            {
                //create activity
                SupplierActivity act = new SupplierActivity();
                act.DtActivity = DateTime.Parse(actDate);
                act.Assigned = Assigned;
                act.Code = Code;
                act.Remarks = Remarks;
                act.SupplierId = int.Parse(SupplierId);
                act.Type = Type;
                act.ActivityType = ActivityType;
                act.Amount = Decimal.Parse(Amount);

                db.SupplierActivities.Add(act);
                db.SaveChanges();

                return "New Activity Added";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        //EDIT - GET : Activities/GetActivityRecord
        [HttpGet]
        public string GetSupActivityRecord(int id)
        {
            try
            {
                //create activity
                SupplierActivity act = db.SupplierActivities.Find(id);
                if (act != null)
                {
                    cUserActivity userAct = new cUserActivity();
                    userAct.Id = act.Id;
                    userAct.Date = act.DtActivity;
                    userAct.SalesCode = act.Code;
                    userAct.Amount = act.Amount;
                    userAct.Remarks = act.Remarks;
                    userAct.Company = act.Supplier.Name;
                    userAct.CustEntMainId = act.SupplierId;
                    userAct.Assigned = act.Assigned;
                    userAct.Type = act.Type;
                    userAct.ActivityType = act.ActivityType;

                    return JsonConvert.SerializeObject(userAct, Formatting.Indented);
                }
                return "ERROR";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //EDIT : Activities/EditActivity
        [HttpPost]
        public string EditSupActivity(int id, string actDate, string Assigned, string Code, string Amount, string Remarks, string SupplierId, string Type, string ActivityType)
        {
            try
            {
                //create activity
                SupplierActivity act = db.SupplierActivities.Find(id);
                act.DtActivity = DateTime.Parse(actDate);
                act.Assigned = Assigned;
                act.Code = Code;
                act.Remarks = Remarks;
                act.SupplierId = int.Parse(SupplierId);
                act.Type = Type;
                act.ActivityType = ActivityType;
                act.Amount = Decimal.Parse(Amount);

                db.Entry(act).State = EntityState.Modified;
                db.SaveChanges();

                return "Edit Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        //DELETE : Activities/DeleteActivityRecord
        [HttpPost]
        public string DeleteSupActivityRecord(int id)
        {
            try
            {
                //create activity
                SupplierActivity act = db.SupplierActivities.Find(id);
                if (act != null)
                {

                    db.SupplierActivities.Remove(act);
                    db.SaveChanges();
                    return "DELETE SUCCESS ";
                }
                return "DELETE ERROR ";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        // GET : User Activity Type report from date range
        [HttpGet]
        public string GetSupTypeSummary(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {

                var custAct = ac.GetSupActivities(user, sDate, eDate);
                var userPerf = ac.GetUserPerformance(custAct, user);

                //convert list to json object
                return JsonConvert.SerializeObject(userPerf, Formatting.Indented);
            }

            return "Error";
        }

        #endregion

        #region Searh Modal 

        [HttpGet]
        public string GetCodeHistoryList(string salesCode)
        {
            var activityList = db.CustEntActivities.Where(a => a.SalesCode.Contains(salesCode))
                .Select(a => new {
                    a.Id,
                    a.CustEntMainId,
                    a.SalesCode,
                    a.ActivityType,
                    a.ProjectName,
                    a.Remarks,
                    a.CustEntMain.Name,
                    a.Status,
                    a.Date.Month,
                    a.Date.Year,
                    a.Date.Day,
                    a.Date,
                    a.Amount
                }).OrderBy(a => a.Date);

            //return Json(activityList.ToList(), JsonRequestBehavior.AllowGet);
            return JsonConvert.SerializeObject(activityList.ToList(), Formatting.Indented);
        }


        [HttpGet]
        public string GetSupCodeHistoryList(string salesCode)
        {
            var activityList = db.SupplierActivities.Where(a => a.Code.Contains(salesCode))
                .Select(a => new {
                    a.Id,
                    a.SupplierId,
                    a.Code,
                    a.ActivityType,
                    a.Supplier.Name,
                    a.Remarks,
                    a.Type,
                    a.DtActivity.Month,
                    a.DtActivity.Year,
                    a.DtActivity.Day,
                    Date = a.DtActivity,
                    a.Amount,
                    a.SupplierActStatu.Status
                }).OrderBy(a => a.Date);

            return JsonConvert.SerializeObject(activityList.ToList(), Formatting.Indented);
        }

        #endregion

        #region Search Activities
        public ActionResult SearchActivities(string srchCode)
        {

            var customerActs = db.CustEntActivities.Where(c => c.SalesCode.Contains(srchCode)).OrderByDescending(c => c.Date).ToList();

            var supplierActs = db.SupplierActivities.Where(c => c.Code.Contains(srchCode)).OrderByDescending(c => c.DtActivity).ToList();

            cActivitySearchResult activitySearchResult = new cActivitySearchResult();
            activitySearchResult.CustEntActivities = customerActs;
            activitySearchResult.SupplierActivities = supplierActs;

            ViewBag.srchCode = srchCode;

            return View(activitySearchResult);

        }
        #endregion

        #region Activites Post Sales

        [Authorize]
        public ActionResult ActivitiesPostSales(string status, string srch, int? statusId)
        {
            var Activities = new List<cActivityPostSales>();

            var user = HttpContext.User.Identity.Name;
            var role = "Admin";

            //handle user roles
            if (!User.IsInRole(role))
            {
                role = "NotAdmin";
            }

            Activities = ac.GetActivityPostSales(status, srch, user, role);

            Activities.ForEach(a => {
                a.ActPostSale = GetLastActPostSale(a.SalesCode);
            });

            if (statusId != null)
            {
                Activities = Activities.Where(a => a.ActPostSale.CustEntActPostSaleStatusId == statusId).ToList();
            }
            else
            {
                Activities = Activities.Where(a => a.ActPostSale.CustEntActPostSaleStatusId == 1).ToList();
            }

            ViewBag.Status = status;
            ViewBag.StatusId = statusId;
            return View(Activities);
        }

        public CustEntActPostSale GetLastActPostSale(string salesCode)
        {
            CustEntActPostSale actPostSale = new CustEntActPostSale();

            var lastPostSale = db.CustEntActPostSales.Where(c => c.SalesCode == salesCode);

            if (lastPostSale.FirstOrDefault() != null)
            {
                var lastest = lastPostSale.OrderByDescending(c => c.DateEncoded).FirstOrDefault();
                actPostSale = lastest;
            }
            else
            {
                actPostSale = new CustEntActPostSale()
                {
                    CustEntActPostSaleStatusId = 1
                };
            }

            return actPostSale;
        }


        // GET: CustEntActivities/Create
        public ActionResult ActivitiesPostSale_Create(string salesCode)
        {
            ViewBag.CustEntActPostSaleStatusId = new SelectList(db.CustEntActPostSaleStatus, "Id", "Status");

            CustEntActPostSale postSale = new CustEntActPostSale();
            postSale.SalesCode = salesCode;
            postSale.DateEncoded = dt.GetCurrentDateTime();
            postSale.By = HttpContext.User.Identity.Name;

            return View(postSale);
        }


        // POST: JobPostSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActivitiesPostSale_Create([Bind("Id,By,DateEncoded,Remarks,SalesCode,CustEntActPostSaleStatusId")] CustEntActPostSale custEntActPostSale)
        {
            if (ModelState.IsValid)
            {
                db.CustEntActPostSales.Add(custEntActPostSale);
                db.SaveChanges();
                return RedirectToAction("ActivitiesPostSales");
            }

            ViewBag.CustEntActPostSaleStatusId = new SelectList(db.CustEntActPostSaleStatus, "Id", "Status");
            return View(custEntActPostSale);
        }

        #endregion

        #region Activity Status tracking 
        [Authorize]
        public ActionResult StatusActivities(string status)
        {

            var Activities = new List<cActivityActiveList>();

            var user = HttpContext.User.Identity.Name;
            var role = "Admin";

            //handle user roles
            if (User.IsInRole(role))
            {
                Activities = ac.GetActiveActivities(status, user, role);
            }
            else
            {
                role = "NotAdmin";
                Activities = ac.GetActiveActivities(status, user, role);
            }

            role = "Admin";

            Activities.ForEach(a => {
                a.Company = db.CustEntMains.Find(a.CompanyId) != null ? db.CustEntMains.Find(a.CompanyId).Name : "N/A";
                a.StatusDoneList = db.CustEntActivities.Where(c => c.SalesCode == a.SalesCode).ToList().Select(c => c.Status);
                a.StatusList = db.CustEntActStatus.ToList().Select(c => c.Status);
            });

            ViewBag.Status = status;
            return View(Activities);


        }

        public ActionResult SupStatusActivities(string status)
        {

            var Activities = new List<cSupActivityActiveList>();

            var user = HttpContext.User.Identity.Name;
            var role = "Admin";

            //handle user roles
            if (User.IsInRole(role))
            {
                Activities = ac.GetSupActiveActivities(status, user, role);
            }
            else
            {
                role = "NotAdmin";
                Activities = ac.GetSupActiveActivities(status, user, role);
            }

            //role = "Admin";

            Activities.ForEach(a => {
                a.StatusDoneList = db.SupplierActivities.Where(c => c.Code == a.Code).ToList().Select(c => c.SupplierActStatu.Status);
                a.StatusList = db.SupplierActStatus.ToList().Select(c => c.Status);
            });

            ViewBag.Status = status;
            return View(Activities);


        }

        public PartialViewResult GetPartialStatusActivities(string status)
        {

            var Activities = new List<cActivityActiveList>();

            var user = HttpContext.User.Identity.Name;
            var role = "Admin";

            //handle user roles
            if (User.IsInRole(role))
            {
                Activities = ac.GetActiveActivities(status, user, role);
            }
            else
            {
                role = "NotAdmin";
                Activities = ac.GetActiveActivities(status, user, role);
            }

            role = "Admin";

            Activities.ForEach(a => {
                a.Company = db.CustEntMains.Find(a.CompanyId) != null ? db.CustEntMains.Find(a.CompanyId).Name : "N/A";
                a.StatusDoneList = db.CustEntActivities.Where(c => c.SalesCode == a.SalesCode).ToList().Select(c => c.Status);
                a.StatusList = db.CustEntActStatus.ToList().Select(c => c.Status);
            });

            ViewBag.Status = status;
            return PartialView(Activities);


        }


        #endregion
    }
}
