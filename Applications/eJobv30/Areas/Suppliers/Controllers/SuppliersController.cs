using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RealSys.CoreLib.Models.Erp;
using RealSys.Modules.SysLib.Lib;
using System.Net;
using RealSys.Modules.SuppliersLib;
using RealSys.CoreLib.Models.DTO.Products;
using RealSys.CoreLib.Models.DTO.Suppliers;
using DevExpress.XtraRichEdit.Import.Html;

namespace eJobv30.Areas.Suppliers.Controllers
{
    [Area("Suppliers")]
    public class SuppliersController : Controller
    {
        private ErpDbContext db;
        private DateClass dt;
        private SupplierClass supdb ;
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" },
                new SelectListItem { Value = "ACC", Text = "Accredited" },
                new SelectListItem { Value = "AOP", Text = "Acc. On Process" }
                };

        public SuppliersController(ErpDbContext _context, ILogger<SuppliersController> _logger)
        {
            db = _context;
            dt = new DateClass();
            supdb = new SupplierClass(_context, _logger);
        }

        // private static string SITECONFIG = ConfigurationManager.AppSettings["SiteConfig"].ToString();
        private static string SITECONFIG = "Realwheels";
        // GET: Suppliers
       // [Authorize]
        public ActionResult Index()
        {
            return View(db.Suppliers.ToList());
        }


        // GET: Suppliers/SupplierItems
        public async Task<IActionResult> Index2()
        {
            var suppliers = db.Suppliers;
            return View(await suppliers.ToListAsync());
        }

        // GET: Suppliers
        // [Authorize]
        public ActionResult Index_VSteel(string search, string category, string status, string sort)
        {
            List<cProductList> supList = supdb.getProductList(search, category, status, "LATEST-DATE");

            return View(supList);
        }

        //Ajax - Table Result 
        //GET the list of suppliers
        public string TableResult(string search, string category, string status, string sort)
        {
            //get supplier list
            List<cSupplierItems> supList = supdb.getSupplierList(search, category, status, sort);

            //convert list to json object
            return JsonConvert.SerializeObject(supList, Formatting.Indented);
        }

        //Ajax - Table Result 
        //GET the list of suppliers 
        public string TableResultProducts(string search, string category, string status, string sort)
        {
            //get supplier list
            List<cProductList> prodList = supdb.getProductList(search, category, status, "LATEST-DATE");

            //convert list to json object
            return JsonConvert.SerializeObject(prodList, Formatting.Indented);
        }

        // GET: Suppliers/Details/5
        //[Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            Supplier supplier = db.Suppliers
                .Include(s => s.City)
                .Include(s => s.Country)
                .Include(s => s.SupplierType)
                .Include(s => s.SupplierContacts)
                .Include(s => s.SupplierInvItems)
                .Where(s=>s.Id == id).FirstOrDefault();

            if (supplier == null)
            {
                return NotFound();
            }

            var supDocListId = db.SupplierDocuments.Where(s => s.SupplierId == id).Select(s => s.SupDocumentId).ToList();

            //current added documents
            var supDocList = db.SupDocuments.Where(s => supDocListId.Contains(s.Id)).ToList();

            var completeDocs = db.SupDocuments.ToList();

            //get documents not added
            IEnumerable<SupDocument> doclist = completeDocs.Except(supDocList);
            ViewBag.Documents = doclist;
            ViewBag.SupplierId = id;
            ViewBag.supContacts = supplier.SupplierContacts.ToList();
            ViewBag.contactStatus = db.SupplierContactStatus.ToList();
            ViewBag.supDocuments = db.SupplierDocuments.Where(s => s.SupplierId == id).Include(s => s.SupDocument);

            InvItemsPartial((int)id);

            return View(supplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name");
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name");

            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId," +
            "SupplierTypeId,Status,Address,CountryId,Website,Code")] Supplier supplier)
        {
            if (ModelState.IsValid && InputValidation(supplier))
            {
                if (HaveNameDuplicate(supplier.Name))
                {
                    ViewBag.Msg = "Supplier Name already exist.";
                    return RedirectToAction("Create");
                }
                else
                {
                    db.Suppliers.Add(supplier);
                    db.SaveChanges();

                    return RedirectToAction("CreateSupContactForm", new { id = supplier.Id });
                }
                //return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", supplier.CountryId);
            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);

            //return View(supplier);
            return RedirectToAction("Details", new { id = supplier.Id });
        }

        // AJAX
        // GET: Suppliers/CreateSupplier
        public ActionResult CreateSupplier()
        {
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name");
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", 175);

            return View();
        }

        public string CreateSupplierSubmit(string Name, string Contact1, string Contact2, string Contact3, string Email, string Details,
            string CityId, string SupplierTypeId, string Status, string Address, string CountryId, string Website, string Code)
        {
            try
            {

                Supplier supplier = new Supplier();
                supplier.Name = Name;
                supplier.Contact1 = Contact1;
                supplier.Contact2 = Contact2;
                supplier.Contact3 = Contact3;
                supplier.Email = Email;
                supplier.Details = Details;
                supplier.CityId = Int32.Parse(CityId);
                supplier.SupplierTypeId = Int32.Parse(SupplierTypeId);
                supplier.Status = Status;
                supplier.Address = Address;
                supplier.CountryId = Int32.Parse(CountryId);
                supplier.Website = Website;
                supplier.Code = Code;

                db.Suppliers.Add(supplier);
                db.SaveChanges();

                return supplier.Id.ToString();
            }
            catch
            {
                return "0";
            }
        }


        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description", supplier.SupplierTypeId);
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", supplier.CountryId);
            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);

            return View(supplier);
        }


        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId,SupplierTypeId,Status,Address,CountryId,Website,Code")] Supplier supplier)
        {
            if (ModelState.IsValid && InputValidation(supplier))
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Suppliers", new { id = supplier.Id });
            }
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", supplier.CountryId);

            return RedirectToAction("Details", "Suppliers", new { id = supplier.Id });
            //return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
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


        //check if supplierName have duplicate
        public bool HaveNameDuplicate(string supName)
        {
            var supDuplicate = db.Suppliers.Where(s => supName.Contains(s.Name)).ToList().Select(s => s.Id);

            if (supDuplicate.Count() != 0)
            {
                //has duplicate
                return true;
            }
            else
            {
                //no duplicate
                return false;
            }
        }

        //check if supplierName have duplicate
        public bool HaveSupNameDuplicate(string supName)
        {
            var supDuplicate = db.SupplierContacts.Where(s => supName.Contains(s.Name)).ToList().Select(s => s.Id);

            if (supDuplicate.Count() != 0)
            {
                //has duplicate
                return true;
            }
            else
            {
                //no duplicate
                return false;
            }
        }

        #region InvItems
        //items of suppliers View
        public ActionResult InvItems(int id)
        {
            InvItemsPartial((int)id);
            ViewBag.Id = id;

            return View(db.SupplierInvItems.Where(s => s.SupplierId == id).ToList());
        }


        public void InvItemsPartial(int id)
        {
            ViewBag.SupplierId = id;
            ViewBag.SupplierName = db.Suppliers.Find(id).Name;
            ViewBag.UnitList = db.SupplierUnits.ToList();

            //get items not added
            var allitems = db.InvItems.ToList();
            //get list of ids of items of supplier
            var supItemsIds = db.SupplierInvItems.Where(d => d.SupplierId == id).ToList().Select(d => d.InvItemId);
            //get list of items of supplier
            var supItems = db.InvItems.Where(d => supItemsIds.Contains(d.Id)).ToList();
            //display items except existing 
            var filteredItems = allitems.Except(supItems).ToList();

            ViewBag.ItemList = filteredItems;

            ViewBag.InvItems = db.SupplierInvItems.Where(s => s.SupplierId == id).ToList();

            ViewBag.InvItemsRates = db.SupplierItemRates.Where(s => s.SupplierInvItem.SupplierId == id).ToList().OrderByDescending(s =>s.DtValidFrom).ToList();

        }

        //POST: /Suppliers/AddInvItems
        //add new items to the supplier
        public ActionResult AddInvItems(int InvID, int supID)
        {
            db.SupplierInvItems.Add(new SupplierInvItem
            {
                InvItemId = InvID,
                SupplierId = supID
            });
            db.SaveChanges();

            return RedirectToAction("Details", "Suppliers", new { id = supID });
        }

        //POST: /Suppliers/AddInvItems
        //remove item(s) from the supplier
        public ActionResult RemoveInvItems(int id)
        {
            SupplierInvItem tempitem = db.SupplierInvItems.Find(id);

            //check and remove existing items
            foreach (var itemRate in tempitem.SupplierItemRates.ToList())
            {
                RemoveRateInvItem(itemRate.Id);
            }

            SupplierInvItem item = db.SupplierInvItems.Find(id);

            db.SupplierInvItems.Remove(item);
            db.SaveChanges();

            return RedirectToAction("InvItems", "Suppliers", new { id = item.SupplierId });
        }

        public bool InputValidation(Supplier supplier)
        {
            bool isValid = true;

            if (supplier.Name == null)
            {
                ModelState.AddModelError("Description", "Invalid Description");
                isValid = false;
            }

            return isValid;
        }
        #endregion

        #region invItemRate

        public bool AddRateInvItems(int id, string Particulars, string Material, string Rate, int Unit, string Remarks,
            string TradeTerm, string Tolerance, string ValidFrom, string ValidTo, string By, string ProcBy, string Origin)
        {
            try
            {
                db.SupplierItemRates.Add(new SupplierItemRate
                {
                    Particulars = Particulars,
                    Material = Material,
                    ItemRate = Rate,
                    SupplierUnitId = Unit,
                    Remarks = Remarks,
                    TradeTerm = TradeTerm,
                    Tolerance = Tolerance,
                    Origin = Origin,
                    DtValidFrom = ValidFrom,
                    DtValidTo = ValidTo,
                    DtEntered = dt.GetCurrentDateTime().ToString(),
                    SupplierInvItemId = id,
                    By = By,
                    ProcBy = ProcBy
                });
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditRateInvItems(int id, string Particulars, string Material, string Rate, int Unit, string ValidFrom, string ValidTo,
            string Remarks, string TradeTerm, string Tolerance, int SupInvId, string By, string ProcBy, string DtEntered, string Origin)
        {
            try
            {

                SupplierItemRate itemRate = db.SupplierItemRates.Find(id);
                itemRate.Particulars = Particulars;
                itemRate.Material = Material;
                itemRate.ItemRate = Rate;
                itemRate.SupplierUnitId = Unit;
                itemRate.TradeTerm = TradeTerm;
                itemRate.Tolerance = Tolerance;
                itemRate.Remarks = Remarks;
                itemRate.DtValidFrom = ValidFrom;
                itemRate.DtValidTo = ValidTo;
                itemRate.SupplierInvItemId = SupInvId;
                itemRate.By = By;
                itemRate.ProcBy = ProcBy;
                itemRate.DtEntered = DtEntered;
                itemRate.Origin = Origin;

                db.Entry(itemRate).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RemoveRateInvItem(int id)
        {
            SupplierItemRate itemRate = db.SupplierItemRates.Find(id);
            db.SupplierItemRates.Remove(itemRate);
            db.SaveChanges();
        }

        #endregion

        #region DeactivateList

        //get list of customers with
        //a year past jobs and no recent jobs 
        public ActionResult DeActivateSupplierList()
        {
            //get supplier list
            List<Supplier> suppliers = supdb.getDeactivatedList();

            return View(suppliers);
        }

        //Deactive a multiple customer by changing its 
        //status from ACT to INC. 
        public ActionResult DeactivateAll()
        {
            //get supplier list
            List<Supplier> suppliers = supdb.getDeactivatedList();

            foreach (var sup in suppliers)
            {
                sup.Status = "INC";    //deactivate customer
                db.Entry(sup).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DeActivateSupplierList", "Suppliers");
        }

        //Deactive a single customer by changing its 
        //status from ACT to INC
        public ActionResult DeactivateSingle(int id)
        {
            var supplier = db.Suppliers.Find(id);
            if (supplier != null)
            {
                supplier.Status = "INC";    //deactivate customer
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("DeactivateSupplierList", "Suppliers");
        }

        #endregion

        #region SupplierContact

        //  Create new Supplier contact
        public ActionResult CreateSupContactForm(int id)
        {
            SupplierContact supContact = new SupplierContact();
            supContact.SupplierId = id;

            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", id);
            ViewBag.SupplierContactStatusId = new SelectList(db.SupplierContactStatus, "Id", "Name");
            return View(supContact);
        }

        //  Create new Supplier contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSupContactForm([Bind("Id,SupplierId,Name,Mobile,Landline,SkypeId,Details,ViberId,WhatsApp,SupplierContactStatusId,Remarks,WeChat,Position,Department")] SupplierContact supplierContact)
        {
            if (ModelState.IsValid && CreateContactValidation(supplierContact))
            {
                if (supplierContact.SupplierId != 0)
                {
                    db.SupplierContacts.Add(supplierContact);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = supplierContact.SupplierId });

            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierContact.SupplierId);
            ViewBag.SupplierContactStatusId = new SelectList(db.SupplierContactStatus, "Id", "Name");
            return View(supplierContact);
        }


        //  Create new Supplier contact
        public string CreateSupContact(int SupplierId, string Name, string Mobile, string Landline, string SkypeId, string ViberId, string WhatsApp, string Email, int Status, string Remarks, string WeChat, string Position, string Department)
        {
            SupplierContact supContact = new SupplierContact();
            try
            {

                supContact.SupplierId = SupplierId;
                supContact.Name = Name;
                supContact.Mobile = Mobile;
                supContact.Landline = Landline;
                supContact.SkypeId = SkypeId;
                supContact.ViberId = ViberId;
                supContact.Remarks = Remarks;
                supContact.WhatsApp = WhatsApp;
                supContact.Email = Email;
                supContact.SupplierContactStatusId = Status;
                supContact.WeChat = WeChat;
                supContact.Position = Position;
                supContact.Department = Department;


                if (CreateContactValidation(supContact))
                {

                    if (SupplierId != 0)
                    {
                        db.SupplierContacts.Add(supContact);
                        db.SaveChanges();
                    }
                    return "True";

                }
                else
                {

                    ViewBag.SupplierId = SupplierId;
                    ViewBag.ContactStatus = db.SupplierContactStatus.ToList();

                    if (supContact.Name == null)
                    {
                        return "Please prove a Name";
                    }
                    else
                    {
                        return "Name is already been used.";
                    }

                }
            }
            catch
            {
                return "False";
            }
        }


        //  Create new Supplier contact
        public string EditSupContact(int id, string Name, string Mobile, string Landline, string SkypeId, string ViberId, string Remarks, string WhatsApp, string Email, int Status, string WeChat, string Position, string Department)
        {
            try
            {

                SupplierContact supContact = db.SupplierContacts.Find(id);
                supContact.Name = Name;
                supContact.Mobile = Mobile;
                supContact.Landline = Landline;
                supContact.SkypeId = SkypeId;
                supContact.ViberId = ViberId;
                supContact.Remarks = Remarks;
                supContact.WhatsApp = WhatsApp;
                supContact.Email = Email;
                supContact.SupplierContactStatusId = Status;
                supContact.WeChat = WeChat;
                supContact.Position = Position;
                supContact.Department = Department;



                if (supContact.Name != null)
                {

                    if (id != 0)
                    {
                        db.Entry(supContact).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return "True";

                }
                else
                {

                    ViewBag.SupplierId = id;
                    ViewBag.ContactStatus = db.SupplierContactStatus.ToList();

                    if (supContact.Name == null)
                    {
                        return "Please prove a Name";
                    }
                    else
                    {
                        return "Name is already been used.";
                    }

                }
            }
            catch
            {
                return "False";
            }

        }

        public ActionResult deleteSupContact(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            SupplierContact supContact = db.SupplierContacts.Find(id);
            if (supContact == null)
            {
                return NotFound();
            }
            int tempId = supContact.SupplierId;

            db.SupplierContacts.Remove(supContact);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = tempId });

        }

        public bool CreateContactValidation(SupplierContact supplierContact)
        {
            bool isValid = true;

            if (supplierContact.Name == null)
            {
                ModelState.AddModelError("Name", "Invalid Name");
                isValid = false;
            }

            if (HaveSupNameDuplicate(supplierContact.Name))
            {

                ModelState.AddModelError("Name", "Name is Already Used");
                isValid = false;
            }

            return isValid;
        }


        #endregion

        #region Documents

        //Add Document to the Supplier Document List
        public ActionResult AddDocuments(int docId, int supId)
        {
            try
            {
                db.SupplierDocuments.Add(new SupplierDocument
                {
                    SupDocumentId = docId,
                    SupplierId = supId
                });
                db.SaveChanges();

            }
            catch
            { }

            return RedirectToAction("Details", "Suppliers", new { id = supId });
        }

        public ActionResult RemoveSupDocument(int id)
        {

            SupplierDocument supplierDoc = db.SupplierDocuments.Find(id);
            var supplierId = supplierDoc.SupplierId;
            db.SupplierDocuments.Remove(supplierDoc);
            db.SaveChanges();

            return RedirectToAction("Details", "Suppliers", new { id = supplierId });

        }

        #endregion
    }
}
