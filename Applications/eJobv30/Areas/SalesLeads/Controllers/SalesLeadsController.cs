using eJobv30.Areas.Suppliers.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RealSys.CoreLib.Models.DTO.SalesLeads;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using RealSys.Modules.SalesLeadLib.Lib;
using RealSys.Modules.SysLib.Lib;
using System.Net;

namespace eJobv30.Areas.SalesLeads.Controllers
{
    [Area("Suppliers")]
    public class SalesLeadsController : Controller
    {
        // NEW CUSTOMER Reference ID
        private int NewCustSysId = 1;

        // Job Status
        private int JOBINQUIRY = 1;
        private int JOBCONFIRMED = 3;

        private enum salesLead_status
        {
            New = 1
        }

        private ErpDbContext db ;
        private DBClasses dbclasses;
        private SalesLeadClass sldb;
        private DateClass date;
        private UserServices userServices;


        public SalesLeadsController(ErpDbContext _context, SysDBContext _sysDBContext,ILogger<SuppliersController> _logger)
        {
            db = _context;
            dbclasses = new DBClasses(_context, _sysDBContext, _logger);
            sldb = new SalesLeadClass(_context, _logger);
            date = new DateClass();
            userServices = new UserServices(_context, _sysDBContext, _logger);
        }

        // GET: SalesLeads
        public ActionResult IndexOld(int? sortid, int? leadId)
        {

            if (sortid != null)
                //Session["SLFilterID"] = (int)sortid;
                HttpContext.Session.SetInt32("SLFilterID", (int)sortid);
            else
            {
                if (HttpContext.Session.GetInt32("SLFilterID") != null)
                    sortid = HttpContext.Session.GetInt32("SLFilterID");
                else
                {
                    sortid = 1;
                    //Session["SLFilterID"] = 1;
                    HttpContext.Session.SetInt32("SLFilterID", 1);
                }
            }

            //get salesl eads leads
            var salesLeads = sldb.GetSalesLeads((int)sortid);

            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 2)
                .OrderBy(s => s.SeqNo).ThenBy(s => s.Id).ToList();
            ViewBag.User = HttpContext.User.Identity.Name;

            //for adding new item 
            ViewBag.InvItems = db.InvItems.ToList();

            return View(salesLeads.OrderByDescending(s => s.Date));
        }



        // GET: SalesLeads
        [Authorize]
        public ActionResult Index(int? sortid, int? leadId)
        {

            if (sortid != null)
                //Session["SLFilterID"] = (int)sortid;
                HttpContext.Session.SetInt32("SLFilterID", (int)sortid);
            else
            {
                if (HttpContext.Session.GetInt32("SLFilterID") != null)
                    sortid = HttpContext.Session.GetInt32("SLFilterID");
                else
                {
                    sortid = 1;
                    //Session["SLFilterID"] = 1;
                    HttpContext.Session.SetInt32("SLFilterID", 1);
                }
            }


            //get sales leads list
            var salesLeads = sldb.GetSalesLeads((int)sortid);

            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 2)
                .OrderBy(s => s.OrderNo).ThenBy(s => s.Id).ToList();
            ViewBag.User = HttpContext.User.Identity.Name;
            ViewBag.ActTypes = db.CustEntActTypes.ToList();
            ViewBag.IsAdmin = IsUserAdmin();
            ViewBag.IsChecker = User.IsInRole("Checker");

            //for adding new item 
            ViewBag.InvItems = db.InvItems.ToList();

            return View(salesLeads.OrderByDescending(s => s.Date));
        }



        //GET: SalesLeads/ForApproval
        [Authorize]
        public ActionResult ForApproval(int? sortid, int? leadId)
        {
            sortid = 4;
            //Session["SLFilterID"] = 4;
            HttpContext.Session.SetInt32("SLFilterID", 4);

            //if (sortid != null)
            //    Session["SLFilterID"] = (int)sortid;
            //else
            //{
            //    if (Session["SLFilterID"] != null)
            //        sortid = (int)Session["SLFilterID"];
            //    else
            //    {
            //        sortid = 4;
            //        Session["SLFilterID"] = 4;
            //    }
            //}

            //if (leadId != null)
            //    Session["SLLeadID"] = (int)leadId;
            //else
            //{
            //    if (Session["SLLeadID"] != null)
            //        leadId = (int)Session["SLLeadID"];
            //}

            if (sortid != null)
                HttpContext.Session.SetInt32("SLFilterID", (int)sortid);
            else
            {
                if (HttpContext.Session.GetInt32("SLFilterID") != null)
                    sortid = HttpContext.Session.GetInt32("SLFilterID");
                else
                {
                    sortid = 1;
                    //Session["SLFilterID"] = 1;
                    HttpContext.Session.SetInt32("SLFilterID", 1);
                }
            }


            if (leadId != null)
                //Session["SLLeadID"] = (int)leadId;
                HttpContext.Session.SetInt32("SLFilterID", (int)leadId);
            else
            {
                if (HttpContext.Session.GetInt32("SLLeadID") != null)
                    leadId = HttpContext.Session.GetInt32("SLLeadID");
            }


            //get sales leads list
            var salesLeads = sldb.GetSalesLeads((int)sortid);

            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 2)
                .OrderBy(s => s.OrderNo).ThenBy(s => s.Id).ToList();
            ViewBag.UnitList = db.SupplierUnits.ToList();
            ViewBag.Suppliers = db.Suppliers.Where(s => s.Status != "INC").OrderBy(s => s.Name).ToList();
            ViewBag.Items = db.InvItems.ToList();
            ViewBag.User = HttpContext.User.Identity.Name;
            ViewBag.ActTypes = db.CustEntActTypes.ToList();
            ViewBag.IsAdmin = IsUserAdmin();
            ViewBag.IsChecker = User.IsInRole("Checker");

            //for adding new item 
            AddSupItemPartial();

            return View(salesLeads.OrderByDescending(s => s.Date));
        }

        public List<SalesStatusCode> GetSalesStatuses()
        {
            var statusList = db.SalesStatusCodes
                 .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 2)
                 .OrderBy(s => s.SeqNo).ThenBy(s => s.Id).ToList();

            //var limit1 = 500000;
            //var limit2 = 3000000;

            return statusList;
        }


        // GET: SalesLeads
        public ActionResult Status(int? sortid, int? leadId)
        {

            //if (sortid != null)
            //    Session["SLFilterID"] = (int)sortid;
            //else
            //{
            //    if (Session["SLFilterID"] != null)
            //        sortid = (int)Session["SLFilterID"];
            //    else
            //    {
            //        sortid = 3;
            //        Session["SLFilterID"] = 3;

            //    }
            //}

            if (sortid != null)
                //Session["SLFilterID"] = (int)sortid;
                HttpContext.Session.SetInt32("SLFilterID", (int)sortid);
            else
            {
                if (HttpContext.Session.GetInt32("SLFilterID") != null)
                    sortid = HttpContext.Session.GetInt32("SLFilterID");
                else
                {
                    sortid = 3;
                    HttpContext.Session.SetInt32("SLFilterID", 3);
                }
            }

            var salesLeads = sldb.GetSalesLeads((int)sortid);

            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .ToList();

            //for adding new item 
            AddSupItemPartial();

            return View(salesLeads.OrderByDescending(s => s.Date));
        }


        // GET: SalesLeads
        public ActionResult IndexCompanies(int? sortid, int? leadId, int? companyId)
        {

            //if (sortid != null)
            //    Session["SLFilterID"] = (int)sortid;
            //else
            //{
            //    if (Session["SLFilterID"] != null)
            //        sortid = (int)Session["SLFilterID"];
            //    else
            //    {
            //        Session["SLFilterID"] = 3;

            //    }
            //}

            if (sortid != null)
                HttpContext.Session.SetInt32("SLFilterID", (int)sortid);
            else
            {
                if (HttpContext.Session.GetInt32("SLFilterID") != null)
                    sortid = HttpContext.Session.GetInt32("SLFilterID");
                else
                {
                    sortid = 3;
                    HttpContext.Session.SetInt32("SLFilterID", 3);
                }
            }

            var leadList = sldb.generateList(null, null, null, null).ToList();

            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes.ToList();

            //for adding new item 
            AddSupItemPartial();

            return View(leadList);
        }


        // GET: SalesLeads
        public ActionResult LeadDetails2(int? sortid, int? id)
        {
            int leadId = (int)id;
            //if (sortid != null)
            //    Session["SLFilterID"] = (int)sortid;
            //else
            //{
            //    if (Session["SLFilterID"] != null)
            //        sortid = (int)Session["SLFilterID"];
            //    else
            //    {
            //        Session["SLFilterID"] = 3;
            //    }
            //}
            if (sortid != null)
                HttpContext.Session.SetInt32("SLFilterID", (int)sortid);
            else
            {
                if (HttpContext.Session.GetInt32("SLFilterID") != null)
                    sortid = HttpContext.Session.GetInt32("SLFilterID");
                else
                {
                    sortid = 3;
                    HttpContext.Session.SetInt32("SLFilterID", 3);
                }
            }

            if (leadId != 0)
            {
                var Id = (int)leadId;
                var salesLeads = db.SalesLeads.Include(s => s.Customer)
                        .Include(s => s.SalesLeadCategories)
                        .Include(s => s.SalesStatus).OrderBy(s => s.Date)
                        .Where(s => s.Id == Id);


                ViewBag.LeadId = leadId;
                ViewBag.CurrentFilter = sortid;
                ViewBag.StatusCodes = db.SalesStatusCodes.ToList();

                //for adding new item 
                AddSupItemPartial();

                return View(salesLeads);
            }
            return RedirectToAction("Index", new { sortid = 1 });

        }

        // GET: SalesLeads
        public ActionResult LeadDetails(int? id, int? sortid)
        {

            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            int leadId = (int)id;
            //if (sortid != null)
            //    Session["SLFilterID"] = (int)sortid;
            //else
            //{
            //    if (Session["SLFilterID"] != null)
            //        sortid = (int)Session["SLFilterID"];
            //    else
            //    {
            //        Session["SLFilterID"] = 3;
            //    }
            //}

            if (sortid != null)
                HttpContext.Session.SetInt32("SLFilterID", (int)sortid);
            else
            {
                if (HttpContext.Session.GetInt32("SLFilterID") != null)
                    sortid = HttpContext.Session.GetInt32("SLFilterID");
                else
                {
                    sortid = 3;
                    HttpContext.Session.SetInt32("SLFilterID", 3);
                }
            }

            if (leadId != 0)
            {
                var Id = (int)id;
                var salesLeads = db.SalesLeads.Include(s => s.Customer)
                        .Include(s => s.SalesLeadCategories)
                        .Include(s => s.SalesStatus).OrderBy(s => s.Date)
                        .Where(s => s.Id == Id);


                ViewBag.LeadId = leadId;
                ViewBag.CurrentFilter = sortid;
                ViewBag.StatusCodes = db.SalesStatusCodes.ToList();
                ViewBag.InvItems = db.InvItems.ToList();
                ViewBag.IsChecker = User.IsInRole("Checker");

                //for adding new item 
                AddSupItemPartial();

                return View(salesLeads);
            }

            return RedirectToAction("Index", new { sortid = 1 });

        }


        // GET: SalesLeads/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            //get salesleads
            var salesLead = db.SalesLeads.Find(id);
            if (salesLead == null)
            {
                return NotFound();
            }


            ViewBag.StatusCodes = db.SalesStatusCodes.ToList();
            ViewBag.Company = salesLead.SalesLeadCompanies.OrderByDescending(s => s.Id).FirstOrDefault().Id;

            ViewBag.LeadId = id;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 2)
                .OrderBy(s => s.OrderNo).ThenBy(s => s.Id).ToList();
            ViewBag.UnitList = db.SupplierUnits.ToList();
            ViewBag.Suppliers = db.Suppliers.Where(s => s.Status != "INC").OrderBy(s => s.Name).ToList();
            ViewBag.Items = db.InvItems.ToList();
            ViewBag.User = HttpContext.User.Identity.Name;
            ViewBag.ActTypes = db.CustEntActTypes.ToList();
            ViewBag.IsAdmin = IsUserAdmin();
            ViewBag.IsChecker = User.IsInRole("Checker");

            //for adding new item 
            AddSupItemPartial();

            return View(salesLead);
        }

        // GET: SalesLeads/Create
        public ActionResult Create()
        {
            var user = HttpContext.User.Identity.Name;

            var tmp = new RealSys.CoreLib.Models.Erp.SalesLead();
            tmp.Date = date.GetCurrentDateTime();
            tmp.DtEntered = date.GetCurrentDateTime();
            tmp.EnteredBy = user;

            ViewBag.CustomerId = new SelectList(db.Customers.Where(s => s.Status == "ACT"), "Id", "Name");
            ViewBag.AssignedTo = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", user);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");

            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList();
            ViewBag.CompanyList = db.CustEntMains.Where(s => s.Status != "INC").ToList();

            return View(tmp);
        }

        // POST: SalesLeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Date,Details,Remarks,Price,CustomerId,CustName,DtEntered," +
            "EnteredBy,AssignedTo,CustPhone,CustEmail,AssignedTo,SalesCode,ItemWeight,Commodity")] SalesLead salesLead, int? CompanyId)
        {
            if (ModelState.IsValid && SalesLeadValidation(salesLead))
            {
                if (CompanyId != null)
                {

                    if (salesLead.EnteredBy== null)
                    {
                        salesLead.EnteredBy = "Guest";
                    }

                    db.SalesLeads.Add(salesLead);
                    db.SaveChanges();

                    AddSalesStatus(salesLead.Id, (int)salesLead_status.New);    //NEW Lead Status

                    AddCompanyLink((int)CompanyId, salesLead.Id);

                    if (salesLead.SalesCode != null)
                    {
                        //if sales code is not null
                        //find customer activities with the same sales code
                        //and add this SalesLead ID to the CustEntActivity entries
                        //UpdateCustActivities(salesLead.Id, salesLead.SalesCode);
                    }

                    return RedirectToAction("Details", new { id = salesLead.Id });
                    //return RedirectToAction("Index", new { sortid = 1 , leadid = salesLead.Id});

                }
                else
                {
                    ModelState.AddModelError("CustomerId", "Invalid Company");
                }
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", salesLead.AssignedTo);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", CompanyId);

            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList();
            ViewBag.CompanyList = db.CustEntMains.Where(s => s.Status != "INC").ToList();

            ViewBag.CompanyName = CompanyId == null ? "" : db.CustEntMains.Find(CompanyId).Name;
            return View(salesLead);
        }

        public void AddCompanyLink(int compId, int leadId)
        {
            SalesLeadCompany slCompany = new SalesLeadCompany();
            slCompany.CustEntMainId = compId;
            slCompany.SalesLeadId = leadId;

            db.SalesLeadCompanies.Add(slCompany);
            db.SaveChanges();
        }

        // GET: SalesLeads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            SalesLead salesLead = db.SalesLeads.Find(id);

            if (salesLead == null)
            {
                return NotFound();
            }

            var company = salesLead.SalesLeadCompanies.OrderByDescending(s => s.Id).FirstOrDefault();

            ViewBag.leadId = id;
            ViewBag.CompanyName = company.CustEntMain.Name;
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", salesLead.AssignedTo);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", company.CustEntMainId);
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList();
            ViewBag.CompanyList = db.CustEntMains.Where(s => s.Status != "INC").ToList();

            return View(salesLead);
        }

        // POST: SalesLeads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Date,Details,Remarks,Price,CustomerId,CustName,DtEntered,EnteredBy," +
            "AssignedTo,CustPhone,CustEmail,AssignedTo,SalesCode,ItemWeight,Commodity")] SalesLead salesLead, int CompanyId)
        {
            if (ModelState.IsValid && SalesLeadValidation(salesLead))
            {
                db.Entry(salesLead).State = EntityState.Modified;
                db.SaveChanges();

                //update salesLead
                updateCompany(CompanyId, salesLead.Id);

                return RedirectToAction("Details", new { id = salesLead.Id });
            }
            var company = db.CustEntMains.Find(CompanyId);

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", salesLead.AssignedTo);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", CompanyId);
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList();
            ViewBag.CompanyList = db.CustEntMains.Where(s => s.Status != "INC").ToList();
            ViewBag.leadId = salesLead.Id;
            ViewBag.CompanyName = company.Name;

            return View(salesLead);
        }

        public void updateCompany(int compId, int leadId)
        {
            SalesLeadCompany slCompany = db.SalesLeadCompanies.Where(s => s.SalesLeadId == leadId).FirstOrDefault();
            slCompany.CustEntMainId = compId;
            db.Entry(slCompany).State = EntityState.Modified;
            db.SaveChanges();
        }

        // GET: SalesLeads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            SalesLead salesLead = db.SalesLeads.Find(id);
            if (salesLead == null)
            {
                return NotFound();
            }
            return View(salesLead);
        }

        // POST: SalesLeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesLead salesLead = db.SalesLeads.Find(id);
            db.SalesLeads.Remove(salesLead);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public bool SalesLeadValidation(SalesLead salesLead)
        {
            bool isValid = true;

            if (salesLead.Date == null)
            {
                ModelState.AddModelError("Date", "Invalid Date");
                isValid = false;
            }

            if (salesLead.Details == null)
            {
                ModelState.AddModelError("Details", "Invalid Description");
                isValid = false;
            }

            if (salesLead.SalesCode == null)
            {
                ModelState.AddModelError("SalesCode", "Invalid SalesCode");
                isValid = false;
            }

            if (salesLead.CustPhone == null)
            {
                ModelState.AddModelError("CustPhone", "Invalid Contact Number");
                isValid = false;
            }

            //if (salesLead.Price == 0 )
            //{
            //    ModelState.AddModelError("Price", "Invalid Price");
            //    isValid = false;
            //}

            else
            {
                if (salesLead.CustPhone.Length < 11)
                {
                    ModelState.AddModelError("CustPhone", "Invalid Contact Number");
                    isValid = false;
                }
            }

            return isValid;
        }

        #region Partial Views / Late Loading

        public ActionResult _PartialProcActivities(int id)
        {
            var activities = db.SalesLeadSupActivities.Where(s => s.SalesLeadId == id).ToList();

            ViewBag.IsAdmin = IsUserAdmin();
            ViewBag.User = HttpContext.User.Identity.Name;

            return PartialView(activities);
        }

        public ActionResult _PartialSupplierItems(int id, string AssignedTo)
        {
            var supItems = db.SalesLeadItems.Where(c => c.SalesLeadId == id).ToList();

            ViewBag.IsAdmin = IsUserAdmin();
            ViewBag.User = HttpContext.User.Identity.Name;
            ViewBag.AssignedTo = AssignedTo;

            return PartialView(supItems);
        }

        //Partial View for Late Loading
        //Param: id = salesLeadId
        public ActionResult _PartialLeadStatus(int id)
        {
            var salesLead = db.SalesLeads.Find(id);

            var statusType = sldb.GetLastActivityType(id);

            ViewBag.IsAdmin = IsUserAdmin();
            ViewBag.User = HttpContext.User.Identity.Name;
            ViewBag.AssignedTo = salesLead.AssignedTo;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 2)
                .OrderBy(s => s.SeqNo).ThenBy(s => s.Id).ToList();
            ViewBag.ActivityStatusType = statusType;

            return PartialView(salesLead);
        }

        #endregion

        [HttpPost]
        public bool UpdateSalesLeadRemarks(int Id, string Remarks)
        {
            try
            {

                var saleslead = db.SalesLeads.Find(Id);

                saleslead.Remarks = Remarks;

                db.Entry(saleslead).State = EntityState.Modified;
                db.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }


        //Get Company Id 
        //Param : id - SalesLead ID
        [HttpGet]
        public string getCompanyId(int custId)
        {
            var custEnt = db.CustEntities.Where(s => s.CustomerId == custId).OrderByDescending(s => s.Id).FirstOrDefault();
            try
            {
                return custEnt.CustEntMainId.ToString();
            }
            catch
            {
                return "1"; //default
            }
        }

        #region Sales Lead Category
        public ActionResult SalesLeadCat(int id)
        {
            var data = db.SalesLeadCategories.Where(d => d.SalesLeadId == id);
            ViewBag.CategoryList = db.SalesLeadCatCodes.ToList();
            ViewBag.SalesLeadId = (int)id;
            return View(data.ToList());
        }

        public ActionResult AddSalesLeadCat(int CodeId, int slId)
        {
            string Message = "";
            try
            {
                //db.Database.ExecuteSqlCommand(@"
                //insert into SalesLeadCategories([SalesLeadCatCodeId],[SalesLeadId])
                //values (" + CodeId.ToString() + "," + slId.ToString() + "); "
                //    );
                //TODO: Insert SalesLead Category

                Message = "Success: " + CodeId.ToString() + "Added...";
                ViewBag.SalesLeadId = slId;
            }
            catch (Exception ex)
            {
                Message = "Error: " + ex.Message;
            }

            return RedirectToAction("SalesLeadCat", new { id = slId });
        }

        public ActionResult RemoveSalesLeadCat(int CodeId, int slId)
        {
            string Message = "";
            try
            {
                //db.Database.ExecuteSqlCommand(@"
                //delete from SalesLeadCategories
                //where (SalesLeadCatCodeId=" + CodeId.ToString() + @"
                //AND SalesLeadId=" + slId.ToString() + "); "
                //    );
                //TODO: Remove SalesLead Category

                Message = "Success: " + CodeId.ToString() + "Removed...";
                ViewBag.SalesLeadId = slId;
            }
            catch (Exception ex)
            {
                Message = "Error: " + ex.Message;
            }

            return RedirectToAction("SalesLeadCat", new { id = slId });
        }
        #endregion

        #region AddSalesStatus
        public ActionResult AddSalesStatus(int slId, int StatusId)
        {
            string strMsg = "";

            if (db.SalesStatus.Where(s => s.SalesLeadId == slId &&
                s.SalesStatusCodeId == StatusId && s.SalesStatusStatusId == 1).FirstOrDefault() == null)
            {

                strMsg = UpdateLeadStatus(slId, StatusId);

                ViewBag.Message = strMsg;

            }

            return RedirectToAction("Index");
        }

        public ActionResult AddSalesStatus_Status(int slId, int StatusId)
        {
            string strMsg = "";

            if (db.SalesStatus.Where(s => s.SalesLeadId == slId &&
                s.SalesStatusCodeId == StatusId && s.SalesStatusStatusId == 1).FirstOrDefault() == null)
            {
                strMsg = UpdateLeadStatus(slId, StatusId);

                ViewBag.Message = strMsg;
            }

            return RedirectToAction("Status");
        }

        //POST: SalesLeads/PostLeadStatus
        [HttpPost]
        public bool PostLeadStatus(int slId, int StatusId)
        {
            string strMsg = "";

            if (db.SalesStatus.Where(s => s.SalesLeadId == slId
                 && s.SalesStatusCodeId == StatusId
                 && s.SalesStatusStatusId == 1)
                .FirstOrDefault() == null)
            {
                strMsg = UpdateLeadStatus(slId, StatusId);
            }

            if (strMsg == "Success")
            {
                return true;
            }

            return false;
        }

        public string UpdateLeadStatus(int slId, int StatusId)
        {
            string strMsg;
            try
            {

                var salesStatus = new SalesStatus();
                salesStatus.DtStatus = date.GetCurrentDateTime();
                salesStatus.SalesLeadId = slId;
                salesStatus.SalesStatusCodeId = StatusId;
                salesStatus.SalesStatusStatusId = 1;

                db.SalesStatus.Add(salesStatus);
                db.SaveChanges();

                strMsg = "Success";

                switch (StatusId)
                {
                    case 1: //New Leads
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        break;
                    case 2:
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        break;
                    case 3:
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        break;
                    case 4:
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        break;
                    case 5:
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        break;
                    case 6:
                        //Awarded
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        AddActivityStatus(slId, "Awarded", "Awarded");
                        break;
                    case 7:
                        //Rejected
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        AddActivityStatus(slId, "Rejected", "Rejected");
                        break;
                    case 8:
                        //Closed
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        AddActivityStatus(slId, "Close", "Closed");
                        break;
                    case 11:
                        //Quotation
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        AddActivityStatus(slId, "Quotation", "Quotation");
                        break;
                    case 15:
                        //Approved by Aldrin
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        RevolveForApprove(slId);
                        break;
                    case 16:
                        //Approved by Mario
                        HttpContext.Session.SetInt32("SLFilterID", StatusId);
                        RevolveForApprove(slId);
                        break;
                    default:
                        break;  //SLFilterID = 3 (All)
                }
            }
            catch (Exception Ex)
            {
                strMsg = "Error:" + Ex.Message;
            }

            return strMsg;
        }

        private void RevolveForApprove(int id)
        {
            var salesLead = db.SalesLeads.Find(id);
            var ApprovedCount = 0;
            var allowedUsers = db.SalesStatusRestrictions.ToList().Select(s => s.SalesStatusCodeId);
            var leadStatus = salesLead.SalesStatus.Where(s => allowedUsers.Contains(s.SalesStatusCodeId)).ToList();

            if (leadStatus.Count() > 0)
            {
                foreach (var approved in leadStatus)
                {
                    ApprovedCount++;
                }
            }

            if (ApprovedCount == 2)
            {
                //mark as approved
                UpdateLeadStatus(id, 5);
            }

        }


        public string GetLeadStatusCount(int statusId)
        {
            try
            {

                var salesLeadCount = sldb.GetSalesLeads((int)statusId).Count();

                if (salesLeadCount > 0)
                {
                    return salesLeadCount.ToString();
                }

                return "";

            }
            catch
            {
                return "";
            }

        }


        public string GetSalesLeadCount(int statusId)
        {
            try
            {

                var salesLeadCount = sldb.GetSalesLeads((int)statusId).Count();

                if (salesLeadCount > 0)
                {
                    return salesLeadCount.ToString();
                }

                return "0";

            }
            catch
            {
                return "0";
            }

        }


        //GET: SalesLeads/GetLastestActivityType 
        public string GetLastestActivityType(int id)
        {
            var salesLead = db.SalesLeads.Find(id);

            var lastActivity = db.CustEntActivities.Where(s => s.SalesCode == salesLead.SalesCode);

            if (lastActivity.FirstOrDefault() != null)
            {
                var activity = lastActivity.OrderByDescending(s => s.Id).FirstOrDefault();

                string activityStatus = activity.CustEntActStatu.Status;
                activityStatus = activityStatus + " - " + activity.Type;

                return activityStatus;
            }

            return "";
        }

        //POST : SalesLead/Revision/{id}
        //id = SalesLead ID
        public ActionResult Revision(int id)
        {

            //get salesLead
            var salesLead = db.SalesLeads.Find(id);

            //get all status 
            var leadstatus = salesLead.SalesStatus.Where(s => s.SalesStatusCodeId < 17).ToList();

            leadstatus.ForEach(s => {
                s.SalesStatusStatusId = 2; //inactive for revision
            });


            foreach (var status in leadstatus)
            {
                db.Entry(status).State = EntityState.Modified;
            }
            //db.Entry(leadstatus).State = EntityState.Modified;
            db.SaveChanges();


            AddSalesStatus(salesLead.Id, 1);    //NEW Lead Status

            return RedirectToAction("Index", new { sortid = 1, leadid = salesLead.Id });
        }

        //POST: Update Sales LEad Activity Data
        public bool UpdateLeadActivityStatus(int id, string status)
        {
            //get sales lead
            try
            {
                var salesLead = db.SalesLeads.Find(id);

                //add sales lead activity to update status
                CustEntActivity activity = new CustEntActivity();
                activity.Date = date.GetCurrentDateTime();
                activity.Assigned = HttpContext.User.Identity.Name;
                activity.SalesCode = salesLead.SalesCode;
                activity.ProjectName = salesLead.Details;
                activity.Amount = salesLead.Price;
                activity.Status = "Open";
                activity.Remarks = status;
                activity.Type = status;
                activity.ActivityType = "Status Update";
                activity.SalesLeadId = id;
                activity.CustEntActStatusId = 1;
                activity.CustEntActActionStatusId = 1;
                activity.CustEntActActionCodesId = 11;
                activity.Commodity = salesLead.Commodity;

                if (salesLead.SalesLeadCompanies.FirstOrDefault() != null)
                {
                    activity.CustEntMainId = salesLead.SalesLeadCompanies.FirstOrDefault().CustEntMainId;
                }

                db.CustEntActivities.Add(activity);
                db.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }

        }

        //Add customer activity status
        public bool AddActivityStatus(int id, string status, string activityStatus)
        {
            //get sales lead
            try
            {
                var salesLead = db.SalesLeads.Find(id);

                //add sales lead activity to update status
                CustEntActivity activity = new CustEntActivity();
                activity.Date = date.GetCurrentDateTime();
                activity.Assigned = HttpContext.User.Identity.Name;
                activity.SalesCode = salesLead.SalesCode;
                activity.ProjectName = salesLead.Details;
                activity.Amount = salesLead.Price;
                activity.Status = activityStatus;
                activity.Remarks = status;
                activity.Type = status;
                activity.ActivityType = "Others";
                activity.SalesLeadId = id;
                activity.CustEntActActionStatusId = 1; //open
                activity.CustEntActActionCodesId = 11; //others
                activity.Commodity = salesLead.Commodity;

                switch (activityStatus)
                {
                    case "Awarded":
                        //activity.CustEntActActionStatusId = 4; //awarded
                        activity.CustEntActStatusId = 4;
                        activity.ActivityType = "Sales";
                        activity.Status = activityStatus;
                        activity.Type = "Buying Inquiry";
                        break;
                    case "Rejected":
                        activity.CustEntActStatusId = 5;
                        activity.Status = "Close";
                        activity.Type = "Others";
                        break;
                    case "Closed":
                        activity.CustEntActStatusId = 5;
                        activity.Status = "Close";
                        activity.Type = "Others";
                        break;
                    case "Quotation":
                        activity.CustEntActStatusId = 1;
                        activity.Status = "Open";
                        activity.Type = "Others";
                        activity.ActivityType = "Quotation";
                        activity.Remarks = "Quotation Sent";
                        break;
                    default:
                        activity.CustEntActStatusId = 1;
                        break;
                }

                if (salesLead.SalesLeadCompanies.FirstOrDefault() != null)
                {
                    activity.CustEntMainId = salesLead.SalesLeadCompanies.FirstOrDefault().CustEntMainId;
                }

                db.CustEntActivities.Add(activity);
                db.SaveChanges();
                return true;

            }
            catch
            {
                //throw ex;
                return false;
            }

        }

        //POST: Update Sales Lead Weight
        public bool UpdateLeadWeight(int id, string weight)
        {
            try
            {
                var salesLead = db.SalesLeads.Find(id);

                salesLead.ItemWeight = weight;

                db.Entry(salesLead).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //GET  SalesLead Weight
        [HttpGet]
        public string GetLeadWeight(int id)
        {
            try
            {
                var salesLead = db.SalesLeads.Find(id);

                return salesLead.ItemWeight;
            }
            catch
            {
                return "0";
            }
        }

        public bool IsUserAdmin()
        {
            var isAdmin = false;

            if (User.IsInRole("Admin") || User.IsInRole("Accounting"))
            {
                isAdmin = true;
            }

            return isAdmin;
        }

        #endregion

        #region Activity Code
        public ActionResult ListActivityCodes(int id)
        {
            var data = db.SalesActCodes.ToList();
            ViewBag.SalesLeadId = id;

            return View(data);
        }
        public ActionResult AddActivityCode(int slId, int ActCodeId)
        {
            var data = new RealSys.CoreLib.Models.Erp.SalesActivity();
            data.SalesLeadId = slId;
            data.SalesActCodeId = ActCodeId;
            data.DtActivity = date.GetCurrentDateTime();
            data.SalesActStatusId = (int)db.SalesActCodes.Find(ActCodeId).DefaultActStatus;
            data.DtEntered = date.GetCurrentDateTime();
            data.EnteredBy = User.Identity.Name;
            data.Particulars = db.SalesActCodes.Find(ActCodeId).Desc;

            ViewBag.SalesActCodeId = new SelectList(db.SalesActCodes, "Id", "Name", ActCodeId);
            ViewBag.SalesLeadId = new SelectList(db.SalesLeads, "Id", "Details", slId);
            ViewBag.SalesActStatusId = new SelectList(db.SalesActStatus, "Id", "Name", data.SalesActStatusId);
            ViewBag.sId = slId;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivityCode([Bind("Id,SalesLeadId,SalesActCodeId,Particulars,DtActivity,SalesActStatusId,DtEntered,EnteredBy")] SalesActivity salesActivity)
        {
            if (ModelState.IsValid)
            {
                db.SalesActivities.Add(salesActivity);
                db.SaveChanges();
                return RedirectToAction("Index", new { leadId = salesActivity.SalesLeadId });
            }

            ViewBag.SalesActCodeId = new SelectList(db.SalesActCodes, "Id", "Name", salesActivity.SalesActCodeId);
            ViewBag.SalesLeadId = new SelectList(db.SalesLeads, "Id", "Details", salesActivity.SalesLeadId);
            ViewBag.SalesActStatusId = new SelectList(db.SalesActStatus, "Id", "Name", salesActivity.SalesActStatusId);
            return View(salesActivity);
        }

        public ActionResult SalesActivityDone(int id)
        {
            db.SalesActivities.FromSqlRaw("update SalesActivities set SalesActStatusId=2 where Id=" + id);
            var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;
            return RedirectToAction("Index", new { leadId = slid });
        }

        public ActionResult SalesActivityRemove(int id)
        {
            var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;
            db.SalesActivities.FromSqlRaw("DELETE FROM SalesActivities where Id=" + id);
            return RedirectToAction("Index", new { leadId = slid });
        }

        //GET: SalesLeads/CheckDuplicateSalesCode?salesCode={salesCode}
        [HttpGet]
        public bool CheckDuplicateSalesCode(string salesCode)
        {
            var salesCodeCount = db.SalesLeads.Where(s => salesCode == s.SalesCode).Count();

            if (salesCodeCount > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Customer

        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        public ActionResult CompanyDetail(int slid, int CustId)
        {
            var data = db.Customers.Find(CustId);

            if (data.Name == "<< New Customer >>")
            {
                return RedirectToAction("CreateCustomer", new { SlId = slid });
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", data.Status);
            ViewBag.leadId = slid;
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyDetail([Bind("Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer, int? leadId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "SalesLeads", new { leadId = leadId });
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);

            return View(customer);
        }



        public ActionResult CreateCustomer(int SlId)
        {
            var dslCust = db.SalesLeads.Find(SlId);
            var data = new RealSys.CoreLib.Models.Erp.Customer();
            data.Name = dslCust.CustName;
            data.Email = dslCust.CustEmail;
            data.Contact1 = dslCust.CustPhone;

            data.Status = "ACT";

            ViewBag.Status = new SelectList(StatusList, "value", "text", data.Status);
            ViewBag.LeadId = SlId;
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer([Bind("Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.Status == null || customer.Status.Trim() == "") customer.Status = "ACT";

                db.Customers.Add(customer);
                db.SaveChanges();

                string SlId = Request.Form["SalesLeadId"];
                db.SalesLeads.FromSqlRaw(@"
                    Update SalesLeads set CustomerId=" + customer.Id + " where Id=" + SlId + ";"
                    );

                return RedirectToAction("Index");
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);
            return View(customer);
        }

        #endregion

        #region Customer Activity 
        public bool UpdateCustActivities(int salesLeadId, string salesCode)
        {
            try
            {

                //find all customer activites with the same sales code
                var custActivities = db.CustEntActivities.Where(c => c.SalesCode == salesCode).ToList();

                //update salesLeadId
                foreach (var act in custActivities)
                {
                    act.SalesLeadId = salesLeadId;

                    db.Entry(act).State = EntityState.Modified;
                    db.SaveChanges();

                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult ListCustActivityCodes(int? id, string salesCode, string projectName, decimal amount, int? companyId)
        {
            if (id != null || companyId != null || salesCode != null)
            {
                var actActionCodes = db.CustEntActActionCodes.ToList();
                ViewBag.SalesLeadId = id;
                ViewBag.SalesCode = salesCode;
                ViewBag.ProjectName = projectName;
                ViewBag.Amount = amount;
                ViewBag.CompanyId = companyId;

                return View(actActionCodes);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddCustActivityCode(int? slId, int ActCodeId, string salesCode, string projectName, decimal amount, int? companyId)
        {
            try
            {
                if (slId != null || companyId != null || salesCode == null)
                {
                    var salesLead = db.SalesLeads.Find(slId);

                    var actCodeDefault = db.CustEntActActionCodes.Find(ActCodeId);

                    CustEntActivity activity = new CustEntActivity();
                    activity.Date = date.GetCurrentDateTime();
                    activity.Assigned = User.Identity.Name;
                    activity.SalesCode = salesCode;
                    activity.ProjectName = projectName;
                    activity.Amount = amount;
                    activity.SalesLeadId = slId;
                    activity.ActivityType = GetActivityType(ActCodeId);
                    activity.CustEntActStatusId = GetActivityStatus(ActCodeId);
                    activity.Commodity = salesLead.Commodity;

                    if (actCodeDefault != null)
                    {
                        activity.Remarks = actCodeDefault.Desc;
                    }

                    ViewBag.Assigned = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", User.Identity.Name);
                    ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", companyId);
                    ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status");
                    ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type");
                    ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", GetActivityType(ActCodeId));
                    ViewBag.CustEntActStatusId = new SelectList(db.CustEntActStatus, "Id", "Status", GetActivityStatus(ActCodeId));
                    ViewBag.CustEntActActionStatusId = new SelectList(db.CustEntActActionStatus, "Id", "ActionStatus", actCodeDefault.DefaultActStatus);
                    ViewBag.CustEntActActionCodesId = new SelectList(db.CustEntActActionCodes, "Id", "Name", ActCodeId);

                    ViewBag.Id = slId;

                    return View(activity);
                }

                return RedirectToAction("Details", new { id = slId });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        private string GetActivityType(int ActCodeId)
        {
            var ActivityType = "";
            switch (ActCodeId)
            {
                case 1:
                    //Quotation 
                    ActivityType = "Quotation";
                    break;
                case 2:
                    //Procurement 
                    ActivityType = "Others";
                    break;
                case 3:
                    //Quotation Done 
                    ActivityType = "Quotation";
                    break;
                case 4:
                    //Call Done
                    ActivityType = "CallsAndEmail";
                    break;
                case 5:
                    //Email Done
                    ActivityType = "CallsAndEmail";
                    break;
                case 6:
                    //Meeting Done 
                    ActivityType = "Meeting";
                    break;
                case 7:
                    //Mgt Approval 
                    ActivityType = "Meeting";
                    break;
                case 8:
                    //Ready For PO 
                    ActivityType = "Others";
                    break;
                case 9:
                    //Awarded
                    ActivityType = "Sales";
                    break;
                case 10:
                    //Closed
                    ActivityType = "Others";
                    break;
                case 11:
                    //Status
                    ActivityType = "Status";
                    break;
                default:
                    //Status Update
                    ActivityType = "Others";
                    break;

            }

            return ActivityType;

        }


        private int GetActivityStatus(int ActCodeId)
        {
            var ActivityStatus = 1;
            switch (ActCodeId)
            {
                case 1:
                    //Quotation 
                    ActivityStatus = 1;
                    break;
                case 2:
                    //Procurement 
                    ActivityStatus = 1;
                    break;
                case 3:
                    //Quotation Done 
                    ActivityStatus = 2;
                    break;
                case 4:
                    //Call Done
                    ActivityStatus = 2;
                    break;
                case 5:
                    //Email Done
                    ActivityStatus = 2;
                    break;
                case 6:
                    //Meeting Done 
                    ActivityStatus = 3;
                    break;
                case 7:
                    //Mgt Approval 
                    ActivityStatus = 3;
                    break;
                case 8:
                    //Ready For PO 
                    ActivityStatus = 3;
                    break;
                case 9:
                    //Awarded
                    ActivityStatus = 4;
                    break;
                case 10:
                    //Closed
                    ActivityStatus = 5;
                    break;
                case 11:
                    //Closed
                    ActivityStatus = 1;
                    break;
                default:
                    //Status Update
                    ActivityStatus = 1;
                    break;

            }

            return ActivityStatus;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustActivityCode([Bind("Id,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,CustEntMainId," +
            "Type,ActivityType,CustEntActStatusId,CustEntActActionStatusId,CustEntActActionCodesId,SalesLeadId,Commodity")]
            CustEntActivity custEntActivity)
        {
            if (ModelState.IsValid)
            {
                custEntActivity.Amount = Decimal.Parse(custEntActivity.Amount.ToString());
                db.CustEntActivities.Add(custEntActivity);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = custEntActivity.SalesLeadId });
            }

            ViewBag.Assigned = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", custEntActivity.Status);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", custEntActivity.Type);
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", custEntActivity.ActivityType);

            ViewBag.CustEntActStatusId = new SelectList(db.CustEntActStatus, "Id", "Status", custEntActivity.CustEntActStatusId);
            ViewBag.CustEntActActionStatusId = new SelectList(db.CustEntActActionStatus, "Id", "ActionStatus", custEntActivity.CustEntActActionStatusId);
            ViewBag.CustEntActActionCodesId = new SelectList(db.CustEntActActionCodes, "Id", "Name", custEntActivity.CustEntActActionCodesId);

            ViewBag.Id = custEntActivity.SalesLeadId;

            return View(custEntActivity);
        }


        public ActionResult EditCustActivityCode(int? id)
        {
            try
            {
                if (id != null)
                {
                    var activity = db.CustEntActivities.Find(id);
                    var salesLead = db.SalesLeads.Find(activity.SalesLeadId);

                    var actCodeDefault = activity.CustEntActActionCode;

                    activity.Date = date.GetCurrentDateTime();
                    activity.Assigned = User.Identity.Name;

                    ViewBag.Assigned = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", User.Identity.Name);
                    ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", activity.CustEntMainId);
                    ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", activity.CustEntActStatu);
                    ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", activity.Type);
                    ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", activity.ActivityType);

                    ViewBag.CustEntActStatusId = new SelectList(db.CustEntActStatus, "Id", "Status", activity.CustEntActStatusId);
                    ViewBag.CustEntActActionStatusId = new SelectList(db.CustEntActActionStatus, "Id", "ActionStatus", actCodeDefault.DefaultActStatus);
                    ViewBag.CustEntActActionCodesId = new SelectList(db.CustEntActActionCodes, "Id", "Name", activity.CustEntActActionCodesId);

                    ViewBag.SlId = activity.SalesLeadId;

                    return View(activity);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustActivityCode([Bind("Id,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,CustEntMainId,Type,ActivityType,CustEntActStatusId,CustEntActActionStatusId,CustEntActActionCodesId")] CustEntActivity custEntActivity)
        {
            if (ModelState.IsValid)
            {
                custEntActivity.Amount = Decimal.Parse(custEntActivity.Amount.ToString());

                db.Entry(custEntActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { leadId = custEntActivity.SalesLeadId });
            }

            ViewBag.Assigned = new SelectList(userServices.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", custEntActivity.Status);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", custEntActivity.Type);
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", custEntActivity.ActivityType);

            ViewBag.CustEntActStatusId = new SelectList(db.CustEntActStatus, "Id", "Status", custEntActivity.CustEntActStatusId);
            ViewBag.CustEntActActionStatusId = new SelectList(db.CustEntActActionStatus, "Id", "ActionStatus", custEntActivity.CustEntActActionStatusId);
            ViewBag.CustEntActActionCodesId = new SelectList(db.CustEntActActionCodes, "Id", "Name", custEntActivity.CustEntActActionCodesId);

            ViewBag.Id = custEntActivity.SalesLeadId;

            return View(custEntActivity);
        }


        public ActionResult CustActivityDone(int id)
        {
            //db.Database.ExecuteSqlCommand("update SalesActivities set SalesActStatusId=2 where Id=" + id);
            //var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;

            var custAct = db.CustEntActivities.Find(id);
            custAct.CustEntActActionStatusId = 2;

            db.Entry(custAct).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { leadId = custAct.SalesLeadId });
        }

        [HttpPost]
        public bool PostCustActivityDone(int id)
        {
            try
            {
                var custAct = db.CustEntActivities.Find(id);
                custAct.CustEntActActionStatusId = 2;

                db.Entry(custAct).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }

        }


        public ActionResult CustActivityRemove(int id)
        {
            //var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;
            //db.Database.ExecuteSqlCommand("DELETE FROM SalesActivities where Id=" + id);

            var custAct = db.CustEntActivities.Find(id);

            custAct.CustEntActActionStatusId = 3;

            db.Entry(custAct).State = EntityState.Modified;
            db.SaveChanges();

            //db.CustEntActivities.Remove(custAct);
            //db.SaveChanges();

            return RedirectToAction("Index", new { leadId = custAct.SalesLeadId });
        }


        [HttpPost]
        public bool PostCustActivityRemove(int id)
        {
            try
            {
                var custAct = db.CustEntActivities.Find(id);

                custAct.CustEntActActionStatusId = 3;

                db.Entry(custAct).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public ActionResult CustActivitiesPartial(int? slId, string salesCode)
        {
            var activities = db.CustEntActivities.Where(c => c.SalesCode == salesCode).ToList();
            var salesLead = db.SalesLeads.Find(slId);
            var user = HttpContext.User.Identity.Name;
            ViewBag.User = user;
            ViewBag.IsAdmin = IsUserAdmin();
            ViewBag.IsAssigned = salesLead.AssignedTo == user ? true : false;

            return PartialView(activities);
        }

        #endregion

        #region SalesLeadFiles

        // GET: SalesLeads/Create
        public ActionResult FileCreate(int custid, int salesleadId)
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", custid);
            ViewBag.SalesleadId = salesleadId;
            return View();
        }

        // POST: SalesLeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileCreate([Bind("Id,Date,Details,Remarks,Price,CustomerId,CustName,DtEntered,EnteredBy,AssignedTo,CustPhone,CustEmail")] SalesLead salesLead)
        {
            if (ModelState.IsValid && salesLead.EnteredBy != null)
            {
                db.SalesLeads.Add(salesLead);
                db.SaveChanges();
                return RedirectToAction("FileList", new { custid = salesLead.CustomerId, salesleadId = salesLead.Id });
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(userServices.getUsers(), "UserName", "UserName", salesLead.AssignedTo);

            return View(salesLead);
        }


        //[HttpPost]
        //public ActionResult UploadFiles(HttpPostedFileBase file, [Bind("Id,Desc,Folder,Path,Remarks,CustomerId")] CustFiles custFiles, int salesleadId)
        //{
        //    if (file != null && file.ContentLength > 0)
        //        try
        //        {

        //            string extension = Path.GetExtension(file.FileName);

        //            //  ~/Images/CustomerFiles/(customerid)/filename.png Path.GetFileName(file.FileName)
        //            string path = Path.Combine(Server.MapPath("~/Images/CustomerFiles/" + custFiles.CustomerId),
        //                                       Path.GetFileName(file.FileName));
        //            string directory = Request.Url.GetLeftPart(UriPartial.Authority) + "/Images/CustomerFiles/" + custFiles.CustomerId + "/";
        //            if (ModelState.IsValid)
        //            {
        //                //add customer
        //                custFiles.Folder = custFiles.CustomerId.ToString(); // ~/customerid
        //                custFiles.Path = directory + Path.GetFileName(file.FileName);
        //                db.CustFiles.Add(custFiles);
        //                db.SaveChanges();

        //                AddFileReference(salesleadId, custFiles.Id);

        //                //create directory if does not exist
        //                var folder = Server.MapPath("~/Images/CustomerFiles/" + custFiles.CustomerId);
        //                if (!Directory.Exists(folder))
        //                {
        //                    Directory.CreateDirectory(folder);
        //                }

        //                file.SaveAs(path);
        //                ViewBag.Message = "File uploaded successfully";
        //            }
        //            else
        //            {
        //                ViewBag.Message = "File uploaded unsuccessfully";
        //                return View("#");
        //            }

        //            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", custFiles.CustomerId);

        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Message = "ERROR:" + ex.Message.ToString();
        //        }
        //    else
        //    {
        //        ViewBag.Message = "You have not specified a file.";
        //    }

        //    return RedirectToAction("FileList", new { custid = custFiles.Id, salesleadId = salesleadId });

        //}

        public void AddFileReference(int RefId, int custfileId)
        {
            db.CustFileRefs.Add(new CustFileRef
            {
                RefTable = "SalesLead",
                RefId = RefId,
                CustFilesId = custfileId
            });
            db.SaveChanges();
        }


        public ActionResult FileList(int custid, int salesleadId)
        {
            List<CustFileRef> Files = db.CustFileRefs.Where(f => f.CustFile.Customer.Id == custid && f.RefId == salesleadId).ToList();
            ViewBag.CustId = custid;
            ViewBag.SalesLeadId = salesleadId;

            return View(Files);
        }

        public ActionResult DeleteFile(int id, int custid, int salesleadId)
        {

            CustFiles custfiles = db.CustFiles.Find(id);
            db.CustFiles.Remove(custfiles);
            db.SaveChanges();

            return RedirectToAction("FileList", new { custid = custid, salesleadId = salesleadId });
        }
        #endregion

        #region Quotation
        public ActionResult QuotationCreate(int id, int custid,
            decimal amount, string cusmail, string contact,
            string desc, string remarks, DateTime leadDT)
        {

            //initial values from 
            JobMain job = new JobMain();
            job.JobDate = System.DateTime.Today;
            job.NoOfDays = 1;
            job.NoOfPax = 1;
            job.AgreedAmt = amount;
            job.CustContactEmail = cusmail;
            job.CustContactNumber = contact;
            job.Description = desc;
            job.JobRemarks = remarks;
            job.JobDate = leadDT;

            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name");
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", 1);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc");
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", custid);
            ViewBag.Id = id;

            return View(job);
        }


        // POST: JobMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuotationCreate([Bind("Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber")] JobMain jobMain, int leadId)
        {
            int NewCustSysId = 1;
            int jobMainId = jobMain.Id;
            if (ModelState.IsValid)
            {
                if (jobMain.CustContactEmail == null && jobMain.CustContactNumber == null)
                {
                    var cust = db.Customers.Find(jobMain.CustomerId);
                    jobMain.CustContactEmail = cust.Email;
                    jobMain.CustContactNumber = cust.Contact1;

                }

                db.JobMains.Add(jobMain);
                // db.SaveChanges();

                db.SalesLeadLinks.Add(new SalesLeadLink
                {
                    JobMainId = jobMain.Id,
                    SalesLeadId = leadId,
                });
                db.SaveChanges();

                if (jobMain.CustomerId == NewCustSysId)
                    return RedirectToAction("CreateCustomer", "JobMains", new { jobid = jobMain.Id });
                else
                    return RedirectToAction("Services", "JobServices", new { id = jobMain.Id });
            }

            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            return View(jobMain);
        }


        // GET: SalesLead/QuotationEdit/5
        public ActionResult QuotationEdit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            JobMain jobMain = db.JobMains.Find(id);
            if (jobMain == null)
            {
                return NotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);

            //TempData["UrlSource"] = Request.UrlReferrer.ToString();
            return View(jobMain);
        }

        // POST: JobMains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuotationEdit([Bind("Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber")] JobMain jobMain)
        {
            if (ModelState.IsValid)
            {
                if (jobMain.CustContactEmail == null && jobMain.CustContactNumber == null)
                {
                    var cust = db.Customers.Find(jobMain.CustomerId);
                    jobMain.CustContactEmail = cust.Email;
                    jobMain.CustContactNumber = cust.Contact1;
                }

                db.Entry(jobMain).State = EntityState.Modified;
                db.SaveChanges();

                //if (jobMain.Customer.Name == "<< New Customer >>")
                if (jobMain.CustomerId == NewCustSysId)
                    return RedirectToAction("CreateCustomer", "JobMains", new { jobid = jobMain.Id });
                else
                    return Redirect((string)TempData["UrlSource"]);
                //return RedirectToAction("Services", "JobServices", new { id = jobMain.Id });
                //return RedirectToAction("Index");

            }
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            return View(jobMain);
        }


        public ActionResult ConfirmJob(int? id)
        {
            JobMain job = db.JobMains.Find(id);
            job.JobStatusId = JOBCONFIRMED;
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();

            //return Redirect(Request.UrlReferrer.ToString());
            return View();
        }


        public ActionResult QuotationLink(int id, int? linkid)
        {
            var links = db.SalesLeadLinks.ToList();
            var main = db.JobMains.Where(s => s.SalesLeadLinks.Where(l => l.JobMainId == s.Id).FirstOrDefault().JobMainId > 0).ToList();
            // Get all anotherHumans where the record does not exist in humans
            ViewBag.links = links;
            var jobMains2 = db.JobMains.Include(j => j.JobSuppliers).Include(j => j.Customer).Include(j => j.Branch).Include(j => j.JobStatus).Include(j => j.JobThru).Include(s => s.SalesLeadLinks).OrderBy(d => d.JobDate);

            IEnumerable<JobMain> leads = jobMains2.ToList();
            if (linkid == 1)
            {
                //joborder
                leads = jobMains2.ToList().Where(d => d.JobStatusId > 1).Except(main);

            }
            else
            {
                //quotation
                leads = jobMains2.ToList().Where(d => d.JobStatusId == JOBINQUIRY).Except(main);

            }

            ViewBag.LeadId = id;

            return View(leads);
        }


        public ActionResult QuotationLinkSelect(int id, int leadId)
        {
            db.SalesLeadLinks.Add(new SalesLeadLink
            {
                JobMainId = id,
                SalesLeadId = leadId
            });
            db.SaveChanges();
            return RedirectToAction("Index", new { leadId = leadId });
        }


        public ActionResult QuotationUnlink(int id)
        {


            SalesLeadLink salesLeadlinks = db.SalesLeadLinks.Where(s => s.SalesLeadId == id).FirstOrDefault(); ;
            db.SalesLeadLinks.Remove(salesLeadlinks);
            db.SaveChanges();
            return RedirectToAction("Index", new { leadId = id });
        }
        #endregion

        #region Items
        public void AddSupItemPartial()
        {
            var items = db.InvItems.ToList();
            ViewBag.InvItems = items;
        }

        public bool addSupItem(int SalesLeadId, int ItemId, decimal price, string remarks)
        {
            try
            {
                SalesLeadItems item = new SalesLeadItems();
                item.InvItemId = ItemId;
                item.SalesLeadId = SalesLeadId;
                item.QuotedPrice = price;
                item.Remarks = remarks;

                db.SalesLeadItems.Add(item);
                db.SaveChanges();

                //Session["SLLeadID"] = SalesLeadId;
                HttpContext.Session.SetInt32("SLLeadID", SalesLeadId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string getItemSuppliers(int id)
        {
            //get list of suppliers of the given item
            var supplier = db.SupplierInvItems.Where(s => s.InvItemId.Equals(id)).ToList();
            List<cItemSupplier> itemSupDetails = new List<cItemSupplier>();

            foreach (var sup in supplier)
            {
                var itemRates = sup.SupplierItemRates.ToList();

                foreach (var rates in itemRates)
                {
                    itemSupDetails.Add(new cItemSupplier
                    {
                        Id = rates.Id,
                        Rate = rates.ItemRate,
                        Particulars = rates.Particulars,
                        Materials = rates.Material,
                        SupplierName = sup.Supplier.Name,
                        Unit = rates.SupplierUnit.Unit,
                        SupRateId = sup.InvItemId.ToString(),
                        ValidStart = rates.DtValidFrom,
                        ValidEnd = rates.DtValidTo
                    });

                }
            }

            //convert list to json object
            return JsonConvert.SerializeObject(itemSupDetails, Formatting.Indented);

        }

        public string AddSupItemRate(int SalesLeadId, int ItemRateId)
        {
            if (SalesLeadId != 0)
            {
                SalesLeadQuotedItem leaditemRate = new SalesLeadQuotedItem();
                leaditemRate.SalesLeadItemsId = SalesLeadId;
                leaditemRate.SupplierItemRateId = ItemRateId;
                leaditemRate.SalesLeadQuotedItemStatusId = 1; //default pending

                db.SalesLeadQuotedItems.Add(leaditemRate);
                db.SaveChanges();

                //Session["SLLeadID"] = SalesLeadId;
                HttpContext.Session.SetInt32("SLLeadID", SalesLeadId);
                return "200";

            }
            return "500";
        }


        public string RemoveSupItemRate(int id)
        {
            try
            {
                SalesLeadQuotedItem leaditemRate = db.SalesLeadQuotedItems.Find(id);
                db.SalesLeadQuotedItems.Remove(leaditemRate);
                db.SaveChanges();

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string RemoveItem(int? id)
        {
            try
            {
                var result = "";
                if (id != null)
                {
                    SalesLeadItems leadItem = db.SalesLeadItems.Find(id);

                    //try to remove children items from the list
                    result = removeSubItems(leadItem);

                    //remove parent item
                    db.SalesLeadItems.Remove(leadItem);
                    db.SaveChanges();
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string removeSubItems(SalesLeadItems items)
        {
            try
            {
                //remove al items from the list
                var leaditems = items.SalesLeadQuotedItems.ToList();
                foreach (var item in leaditems)
                {
                    RemoveSupItemRate(item.Id);
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string EditItem(int id, int invItemId, decimal rate, string remarks)
        {
            try
            {
                var leaditems = db.SalesLeadItems.Find(id);
                leaditems.InvItemId = invItemId;
                leaditems.QuotedPrice = rate;
                leaditems.Remarks = remarks;

                db.Entry(leaditems).State = EntityState.Modified;
                db.SaveChanges();

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string GetSalesStatusAllowedUser(int statusCodeId)
        {
            var user = HttpContext.User.Identity.Name;

            var restriction = db.SalesStatusRestrictions.Where(s => s.SalesStatusCodeId == statusCodeId).ToList();

            if (restriction.Count() > 0)
            {
                if (restriction.Where(s => s.SalesStatusAllowedUser.User == user).Count() > 0)
                {
                    return "True";
                }
            }
            return "False";
        }


        public string GetUserName()
        {
            var userLogin = User.Identity.Name;

            return userLogin.Split('@')[0];
        }


        #endregion

        #region Lead Link

        // GET:    /SalesLeads/GetLeadLastestLink/{id}
        // param:  id = (int) salesLeadId
        // return: (string) File link 
        // Get Sales Lead Latest File Link
        [HttpGet]
        public JsonResult GetLeadLastestLink(int? id)
        {
            try
            {
                if (id == null)
                {
                    return Json("", System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }

                var listOfLinks = db.SalesLeadFiles.Where(s => s.SalesLeadId == id);

                if (listOfLinks != null)
                {
                    var latestLink = listOfLinks.ToList().OrderByDescending(s => s.Id).FirstOrDefault();

                    return Json(new
                    {
                        latestLink.Id,
                        latestLink.Link
                    }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }

                return Json("", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public int GetLeadLinkCount(int? id)
        {
            try
            {
                if (id == null)
                {
                    return 0;
                }

                var listOfLinks = db.SalesLeadFiles.Where(s => s.SalesLeadId == id).ToList();

                if (listOfLinks != null)
                {
                    return listOfLinks.Count();
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        //POST: SalesLeads/AddLeadLink/{id}&link={link}
        public string AddLeadLink(int? id, string link)
        {
            try
            {

                if (id == null)
                {
                    return "No Id found.";
                }

                SalesLeadFile leadFile = new SalesLeadFile();
                leadFile.SalesLeadId = (int)id;
                leadFile.Link = link;

                db.SalesLeadFiles.Add(leadFile);
                db.SaveChanges();

                return "OK";
            }
            catch
            {
                return "Error Adding Lead Link";
            }
        }


        //POST: SalesLeads/EditLeadLink/{id}&link={link}
        //param: id = SalesLeadFileId  (int)
        //       link = link (string)
        //return: string message
        public string EditLeadLink(int? id, string link)
        {
            try
            {

                if (id == null)
                {
                    return "No Id found.";
                }

                SalesLeadFile leadFile = db.SalesLeadFiles.Find(id);

                if (leadFile == null)
                {
                    return "No File found.";
                }

                //update file link
                leadFile.Link = link;

                //save changes
                db.Entry(leadFile).State = EntityState.Modified;
                db.SaveChanges();

                return "OK";
            }
            catch
            {
                return "Error Updating Lead Link";
            }
        }

        #endregion

    }
}
