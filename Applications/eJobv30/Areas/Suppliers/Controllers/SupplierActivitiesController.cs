using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using RealSys.Modules.SuppliersLib;
using RealSys.Modules.SysLib.Lib;
using System.Net;

namespace eJobv30.Areas.Suppliers.Controllers
{
    [Area("Suppliers")]
    public class SupplierActivitiesController : Controller
    {
        private ErpDbContext db;
        private DBClasses dbclasses ;
        private SupplierClass supdb;
        private DateClass date;
        private UserServices UserServices;

        private List<SelectListItem> ActivityType = new List<SelectListItem> {
                new SelectListItem { Value = "Meeting", Text = "Meeting" },
                new SelectListItem { Value = "Quotation", Text = "Quotation" },
                new SelectListItem { Value = "Sales", Text = "Sales" }
        };

        public SupplierActivitiesController(ErpDbContext _context, SysDBContext _sysDBContext,  ILogger<SuppliersController> _logger, UserManager<IdentityUser> _userManager)
        {
            db = _context;
            dbclasses = new DBClasses(_context, _sysDBContext, _logger);
            supdb = new SupplierClass(_context, _logger);
            date = new DateClass();
            UserServices = new UserServices(_context, _sysDBContext, _logger, _userManager);
        }

        // GET: SupplierActivities/{index}
        public ActionResult Index()
        {
            var supplierActivities = db.SupplierActivities.Include(s => s.Supplier);
            return View(supplierActivities.OrderByDescending(s => s.DtActivity).ToList());
        }

        // GET: SupplierActivities/{index}
        public ActionResult Records(int id)
        {
            var supplierActivities = db.SupplierActivities.Where(s => s.SupplierId == id).Include(s => s.Supplier).ToList();

            ViewBag.SupplierName = db.Suppliers.Find(id).Name;
            ViewBag.Id = id;

            foreach (var act in supplierActivities)
            {
                act.Assigned = supdb.removeEmailString(act.Assigned);
            }

            return View(supplierActivities);
        }

        // GET: SupplierActivities/Details/5
        public ActionResult Details(int? id)
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
            return View(supplierActivity);
        }

        // GET: SupplierActivities/Create
        public ActionResult Create()
        {
            ViewBag.Assigned = new SelectList(UserServices.getUsers_wdException(), "UserName", "UserName");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            return View();
        }

        // POST: SupplierActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Code,DtActivity,Assigned,Remarks,SupplierId,Amount")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                supplierActivity.Type = "Meeting"; // default for all supplier activity history
                db.SupplierActivities.Add(supplierActivity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Assigned = new SelectList(UserServices.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            return View(supplierActivity);
        }

        // GET: SupplierActivities/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Assigned = new SelectList(UserServices.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Code,DtActivity,Assigned,Remarks,SupplierId,Amount")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierActivity).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Records", new { id = supplierActivity.SupplierId });
            }
            ViewBag.Assigned = new SelectList(UserServices.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            return View(supplierActivity);
        }

        // GET: SupplierActivities/Delete/5
        public ActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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

            return RedirectToAction("Records", new { id = supplierActivity.SupplierId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // GET: SupplierActivities/Create
        public ActionResult RecordsCreate(int id)
        {
            ViewBag.Assigned = new SelectList(UserServices.getUsers_wdException(), "UserName", "UserName");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", id);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type");
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type");
            ViewBag.SupplierActStatusId = new SelectList(db.SupplierActStatus, "Id", "Status");
            ViewBag.Currency = new SelectList(db.Currencies, "Name", "Name");

            SupplierActivity supAct = new SupplierActivity();
            supAct.DtActivity = date.GetCurrentDateTime();

            return View(supAct);
        }

        // POST: SupplierActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordsCreate([Bind("Id,Code,DtActivity,Assigned,Amount,Remarks,SupplierId,Amount,Type,ActivityType,SupplierActStatusId,Currency")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                supplierActivity.SupplierActActionCodeId = 1; //default
                supplierActivity.SupplierActActionStatusId = 1; //default

                db.SupplierActivities.Add(supplierActivity);
                db.SaveChanges();
                return RedirectToAction("Records", new { id = supplierActivity.SupplierId });
            }

            ViewBag.Assigned = new SelectList(UserServices.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", supplierActivity.Type);
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type", supplierActivity.ActivityType);
            ViewBag.SupplierActStatusId = new SelectList(db.SupplierActStatus, "Id", "Status", supplierActivity.SupplierActStatusId);
            ViewBag.Currency = new SelectList(db.Currencies, "Name", "Name");
            return View(supplierActivity);
        }


        // GET: SupplierActivities/Edit/5
        public ActionResult RecordsEdit(int? id)
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

            ViewBag.Assigned = new SelectList(UserServices.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", supplierActivity.Type);
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type", supplierActivity.ActivityType);
            ViewBag.SupplierActStatusId = new SelectList(db.SupplierActStatus, "Id", "Status", supplierActivity.SupplierActStatusId);
            ViewBag.SupplierActActionCodeId = new SelectList(db.SupplierActActionCodes, "Id", "Name", supplierActivity.SupplierActActionCodeId);
            ViewBag.SupplierActActionStatusId = new SelectList(db.SupplierActActionStatus, "Id", "ActionStatus", supplierActivity.SupplierActActionStatusId);
            ViewBag.Currency = new SelectList(db.Currencies, "Name", "Name", supplierActivity.Currency);

            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordsEdit([Bind("Id,Code,DtActivity,Assigned,Amount,Remarks,SupplierId,Amount,Type,ProjName,ActivityType,SupplierActStatusId,SupplierActActionCodeId,SupplierActActionStatusId,Currency")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {

                db.Entry(supplierActivity).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Records", new { id = supplierActivity.SupplierId });
            }

            ViewBag.Assigned = new SelectList(UserServices.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", supplierActivity.Type);
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type", supplierActivity.ActivityType);
            ViewBag.SupplierActStatusId = new SelectList(db.SupplierActStatus, "Id", "Status", supplierActivity.SupplierActStatusId);
            ViewBag.SupplierActActionCodeId = new SelectList(db.SupplierActActionCodes, "Id", "Name", supplierActivity.SupplierActActionCodeId);
            ViewBag.SupplierActActionStatusId = new SelectList(db.SupplierActActionStatus, "Id", "ActionStatus", supplierActivity.SupplierActActionStatusId);
            ViewBag.Currency = new SelectList(db.Currencies, "Name", "Name", supplierActivity.Currency);

            return View(supplierActivity);
        }
    }
}

