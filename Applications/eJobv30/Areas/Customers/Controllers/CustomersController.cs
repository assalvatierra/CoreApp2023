using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.DTO.Customers;
using eJobv30.Data;
using RealSys.Modules.SysLib.Lib;
using RealSys.Modules.CustomersLib.Lib;
using RealSys.CoreLib.Interfaces.System;
using RealSys.CoreLib.Models.DTO.Customers;

namespace eJobv30.Controllers
{
    //Areas
    [Area("Customers")]
    public class CustomersController : Controller
    {
        private ErpDbContext db;

        private CustomerClass custdb;
        private CustAgentClass agentClass;
        private RealSys.Modules.CustomersLib.Lib.JobVehicleClass jvc;

        public CustomersController(ILogger<CustomersController> logger, ErpDbContext erpDb)
        {
            db = erpDb;
            custdb = new CustomerClass(db);
            agentClass = new CustAgentClass(db);
            jvc = new RealSys.Modules.CustomersLib.Lib.JobVehicleClass(db);
        }



        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "RES", Text = "Resigned" },
                new SelectListItem { Value = "TRN", Text = "Transferred" }
                };

        //  private string SITECONFIG = ConfigurationManager.AppSettings["SiteConfig"].ToString();
        private string SITECONFIG = "RealWheels";

        public ActionResult IndexSample()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers
       // [Authorize]
        public ActionResult Index(string status, string search)
        {

            List<CustomerDetails> customerDetailList = new List<CustomerDetails>();
            //get customer list async
            customerDetailList = custdb.getCustomerList(status, search);

            ViewBag.status = status;
            ViewBag.SiteConfig = SITECONFIG;
            return View(customerDetailList.OrderBy(s => s.Name));
        }

        // GET: Customers/Details/5
       // [Authorize]
        public ActionResult Details(int? id, int? top, int? last, string sdate, string edate, string status, string sortdate)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            if (top == null)
            {
                top = 30;
            }

            //generate partial view list for companies
            PartialView_Companies(id);
            PartialView_Jobs((int)id, (int)top, sdate, edate, status, sortdate);
            PartialView_Categories(id);
            PartialView_CustomerFiles(id);
            PartialView_CustSocial((int)id);

            //
            ViewBag.categoryList = db.CustCategories.ToList();
            ViewBag.custId = (int)id;
            ViewBag.HaveJob = db.JobMains.Where(j => j.CustomerId == id).FirstOrDefault() != null ? true : false;
            ViewBag.SiteConfig = SITECONFIG;
            ViewBag.CustomerVehicles = db.Vehicles.Where(v => v.CustomerId == id).OrderBy(v => v.VehicleModel.VehicleBrand.Brand).ToList();
            ViewBag.VehicleModelList = db.VehicleModels.OrderBy(v => v.VehicleBrand.Brand).ThenBy(v => v.Make).ToList();

            //check previlages
            var isAdmin = User.IsInRole("Admin");
            var isServiceAdvisor = User.IsInRole("ServiceAdvisor");
            ViewBag.IsAllowedVehicles = isAdmin || isServiceAdvisor ? true : false;

            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            //ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.Status = StatusList;

            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer, string socialAcc)
        {
            if (ModelState.IsValid)
            {
                if (customer.Status == null || customer.Status.Trim() == "") customer.Status = "ACT";

                if (HaveNameDuplicate(customer.Name))
                {
                    ViewBag.Msg = "Customer Name already exist.";
                    return RedirectToAction("Create");
                }
                else
                {

                    db.Customers.Add(customer);
                    db.SaveChanges();

                    //socialAcc = "fb.com/melissa";
                    //create social account
                    custdb.CreateSocialAccount(customer.Id, socialAcc);

                    return RedirectToAction("Details", new { id = customer.Id });
                }
            }

            return View(customer);
        }


        // GET: Customers/Create
        public ActionResult CreateAgent(int custEntMainId)
        {
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.Type = new SelectList(db.CustAssocTypes, "Id", "Type", 2);
            ViewBag.custEntMainId = custEntMainId;
            ViewBag.AgentList = db.CustEntities.Where(c => c.CustAssocTypeId == 2).Select(c => c.Customer).ToList();

            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAgent([Bind("Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer,
            string socialAcc, int CustEntMainId, string AgentCompany, string AgentPosition, int? CustAgentId)
        {
            if (ModelState.IsValid)
            {
                if (customer.Status == null || customer.Status.Trim() == "") customer.Status = "ACT";

                if (CustAgentId == null || CustAgentId == 0)
                {

                    db.Customers.Add(customer);
                    db.SaveChanges();

                    //create social account
                    custdb.CreateSocialAccount(customer.Id, socialAcc);

                    //create new Agent
                    agentClass.CreateAgent(customer.Id, CustEntMainId, AgentCompany, AgentPosition);
                }
                else
                {
                    //assign new Agent to company
                    agentClass.CreateAgent((int)CustAgentId, CustEntMainId, AgentCompany, AgentPosition);
                }

                return RedirectToAction("Details", "CustEntMains", new { id = CustEntMainId });

            }

            ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.Type = new SelectList(db.CustAssocTypes, "Id", "Type", 2);

            return View(customer);
        }


        // GET: Customers/CreateCustomer
        public ActionResult CreateCustomer()
        {
            //ViewBag.Status = new SelectList(StatusList, "value", "text");

            ViewBag.Status = StatusList;

            return View();
        }
        public string CreateCustomerAjax(string Name, string Email, string Contact1, string Contact2, string Remarks, string Status)
        {
            try
            {

                Customer customer = new Customer();
                customer.Name = Name;
                customer.Email = Email;
                customer.Contact1 = Contact1;
                customer.Contact2 = Contact2;
                customer.Remarks = Remarks;
                customer.Status = Status;

                db.Customers.Add(customer);
                db.SaveChanges();

                return customer.Id.ToString();

            }
            catch
            {
                return "0";
            }
        }


        // GET: Customers/CompanyCreate
        public ActionResult CompanyCreate()
        {
            return View();
        }

        // POST: Customers/CompanyCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyCreate([Bind("Id,Name,Address,Contact1,Contact2,iconPath")] CustEntMain custEntMain, int? id)
        {
            if (ModelState.IsValid)
            {

                db.CustEntMains.Add(custEntMain);
                db.SaveChanges();

                //save new company to customer
                CustEntity company = new CustEntity();
                company.CustEntMainId = custEntMain.Id;
                company.CustomerId = (int)id;
                db.CustEntities.Add(company);
                db.SaveChanges();

                return RedirectToAction("Details", "Customers", new { id });
            }

            return View(custEntMain);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            //ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);
            ViewBag.Status = StatusList;

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = customer.Id });
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);

            return View(customer);
        }


        // GET: Customers/Edit/5
        public ActionResult EditAgent(int? id, int? custEntMainId, int? custEntId)
        {
            if (id == null || custEntMainId == null || custEntId == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            CustEntity agent = db.CustEntities.Find(custEntId);
            if (agent == null)
            {
                return NotFound();
            }


            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);
            ViewBag.CustEntMainId = custEntMainId;
            ViewBag.CustEntId = custEntId;
            ViewBag.AgentCompany = agent.Company;
            ViewBag.AgentPosition = agent.Position;

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAgent([Bind("Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer,
            int CustEntMainId, string AgentCompany, string AgentPosition, int CustEntId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();

                agentClass.EditAgent(CustEntId, CustEntMainId, AgentCompany, AgentPosition);

                return RedirectToAction("Details", "CustEntMains", new { id = CustEntMainId });
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);
            ViewBag.AgentCompany = AgentCompany;
            ViewBag.AgentPosition = AgentPosition;

            return View(customer);
        }


        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        // GET: Customers/RemoveAgent/5
        public ActionResult RemoveAgent(int? id, int custEntMainId)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            CustEntity agent = db.CustEntities.Find(id);
            if (agent == null)
            {
                return NotFound();
            }

            ViewBag.custEntMainId = custEntMainId;
            return View(agent);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("RemoveAgent")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAgent(int id)
        {
            CustEntity agent = db.CustEntities.Find(id);

            int companyId = agent.CustEntMainId;

            db.CustEntities.Remove(agent);
            db.SaveChanges();

            return RedirectToAction("Details", "CustEntMains", new { id = companyId });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //---------------------------------------------//

        //returns the companies of the customers
        //using Viewbags
        private void PartialView_Companies(int? id)
        {
            if (id != null)
            {


                //PartialView for Details of the Customer
                List<CustEntMain> CompanyList = custdb.getCustCompanies((int)id);

                ViewBag.companyList = CompanyList.ToList();

                //get all companies
                List<CustEntMain> List = db.CustEntMains.ToList();

                ViewBag.companies = List;

                var Companies = db.CustEntities.Include(c=>c.CustEntMain).Where(s => s.CustomerId == id).ToList();

                try
                {
                    //check if there is company linked ot customer
                    var RecentCompany = db.CustEntities.Include(c => c.CustEntMain).Where(s => s.CustomerId == id).OrderByDescending(s => s.Id).FirstOrDefault();

                    var PositionCompany = "";
                    if (RecentCompany != null)
                    {
                        PositionCompany = RecentCompany.Position;
                    }
                    else
                    {
                        ViewBag.custposition = "N/A";
                    }
                    ViewBag.custposition = PositionCompany;
                }
                catch
                {
                    ViewBag.custposition = "N/A";
                }

                //var position = db.CustEntities.Where(s=>s.CustomerId == id && s.CustEntMainId == RecentCompany.CustEntMainId)


                ViewBag.companiesPrev = Companies;
                ViewBag.CustomerID = id;

            }
        }

        //customer jobs list
        private void PartialView_Jobs(int id, int top, string sdate, string edate, string status, string sortdate)
        {
            //get customer jobs and display to job list table
            ViewBag.jobList = custdb.getCustomerJobList(id, top, sdate, edate, status, sortdate);
        }

        //display list of categories assigned 
        //to the customer 
        private void PartialView_Categories(int? id)
        {
            //get list of categories
            ViewBag.categoryDetails = custdb.getCategoriesList((int)id);
            ViewBag.categoryList = db.CustCategories.ToList();
        }

        //display list of customer files 
        //uploaded on the system
        //Note: not working due to the 
        //problem adding files to online database
        private void PartialView_CustomerFiles(int? id)
        {
            //PartialView for Details of the Customer
            List<CustFiles> FilesList = custdb.getCustFiles((int)id);

            ViewBag.fileList = FilesList;
        }

        // Customers/Details
        // assign company Category to the user
        public string addCompanyCat(int companyId, int userid, string position)
        {
            db.CustEntities.Add(new CustEntity
            {
                CustEntMainId = companyId,
                CustomerId = userid,
                Position = position
            });
            db.SaveChanges();

            return "200";
        }

        // /Customers/Details
        // edit company of user
        public string editCompanyUser(int companyId, int userid, string position)
        {
            //find latest
            if (db.CustEntities.Where(c => c.CustEntMainId == companyId && c.CustomerId == userid) != null)
            {
                CustEntity tempEnt = db.CustEntities.Where(c => c.CustEntMainId == companyId && c.CustomerId == userid).OrderByDescending(c => c.Id).FirstOrDefault();
                tempEnt.Position = position;
                db.Entry(tempEnt).State = EntityState.Modified;
            }
            else
            {
                //if no record, add new 
                db.CustEntities.Add(new CustEntity
                {
                    CustEntMainId = companyId,
                    CustomerId = userid,
                    Position = position
                });
            }

            db.SaveChanges();
            return "OK";
        }

        //get list of customers with
        //a year past jobs and no recent jobs 
        public ActionResult DeactivateOldCustomer()
        {

            //get list of customers which are candidate for deactivation
            List<Customer> allCustomers = custdb.getDeactivateCustomers();

            //Display to view customers
            return View(allCustomers);
        }


        //Deactive a multiple customer by changing its 
        //status from ACT to INC. 
        public ActionResult DeActivateAll()
        {
            //get list of customers which are candidate for deactivation
            List<Customer> dCustList = custdb.getDeactivateCustomers();

            //deactivate customers by changing its 
            //status from ACT to INC
            foreach (var customer in dCustList)
            {
                var deleteCust = db.Customers.Find(customer.Id);
                deleteCust.Status = "INC";    //deactivate customer
                db.Entry(deleteCust).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DeactivateOldCustomer", "Customers");
        }

        //Deactivate a single customer by changing its 
        //status from ACT to INC
        public ActionResult DeactivateSingle(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer != null)
            {
                customer.Status = "INC";    //deactivate customer
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DeactivateOldCustomer", "Customers");
        }

        //get current time based on Singapore Standard Time 
        //SGT - UTC +8
        protected DateTime GetCurrentTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }

        //Ajax - Table Result 
        //Get the list of customers containing the search string,
        //if search is empty, return all actve items
        //Param : search = search string
        //        status = customer list string
        public string TableResult(string search, string status, string sort)
        {
            //get lit of customers
            List<CustomerList> custList = new List<CustomerList>();
            var user = HttpContext.User.Identity.Name;

            if (SITECONFIG == "AutoCare")
            {
                custList = custdb.GetCustomerAdminList(search, status, sort);
            }
            else
            {
                //handle user roles
                //if (User.IsInRole("Admin"))
                //{
                //    custList = custdb.GetCustomerAdminList(search, status, sort);
                //}
                //else
                //{
                //    custList = custdb.GetCustomerList(search, status, sort, user);
                //}

                custList = custdb.GetCustomerAdminList(search, status, sort);
            }

            //convert list to json object
            return JsonConvert.SerializeObject(custList, Formatting.Indented);
        }


        //check if Customer Name have duplicate
        public bool HaveNameDuplicate(string custName)
        {
            var custDuplicate = db.Customers.Where(s => custName == s.Name).ToList().Count();

            if (custDuplicate != 0)
            {
                //has duplicate
                return true;
            }
            else
            {
                //count = 0
                //no duplicate
                return false;
            }
        }


        //GET : /Customers/GetCustomerByCompanyId
        //id = companyId
        //return Json: first customer name, contact number and email
        [HttpGet]
        public JsonResult GetCustomerByCompanyId(int id)
        {
            try
            {
                var company = db.CustEntMains.Find(id);

                var customerList = company.CustEntities;

                if (customerList == null)
                {
                    return Json(null);
                }

                var defaultCustomer = customerList.FirstOrDefault().Customer;


                return Json(new
                {
                    defaultCustomer.Id,
                    defaultCustomer.Name,
                    defaultCustomer.Contact1,
                    defaultCustomer.Contact2,
                    defaultCustomer.Email,
                    Company = company.Name
                });
            }
            catch
            {
                return null;
            }
        }


        //GET : /Customers/GetAgentCompanyDetails
        [HttpGet]
        public JsonResult GetAgentCompanyDetails(int id)
        {
            try
            {
                var custAgent = db.Customers.Find(id);

                if (custAgent.CustEntities.Count() > 0)
                {

                    return Json(new
                    {
                        custAgent.Id,
                        custAgent.Name,
                        custAgent.CustEntities.First().Company,
                        custAgent.CustEntities.First().Position

                    });

                }

                return null;
            }
            catch
            {
                return null;
            }
        }


        #region Customer Social details

        //display list of categories assigned 
        //to the customer 
        private void PartialView_CustSocial(int id)
        {
            //get list of categories
            ViewBag.CustSocial = db.CustSocialAccs.Where(s => s.CustomerId == id).ToList();
        }

        public string CreateCustSocial(int custId, string facebook, string skypeId, string viberId)
        {
            try
            {

                db.CustSocialAccs.Add(new CustSocialAcc
                {
                    Facebook = facebook,
                    Skype = skypeId,
                    Viber = viberId,
                    CustomerId = custId
                });
                db.SaveChanges();

                return "OK : ";
            }
            catch
            {
                return "Cannot Process adding new social details.";
            }
        }

        // POST: Customers/Delete/5
        public ActionResult DeleteCustSocial(int id)
        {
            try
            {
                CustSocialAcc custSocial = db.CustSocialAccs.Find(id);
                var customerid = custSocial.CustomerId;
                db.CustSocialAccs.Remove(custSocial);
                db.SaveChanges();
                return RedirectToAction("Details", "Customers", new { id = customerid });
            }
            catch
            {
                return RedirectToAction("Details", "Customers");
            }
        }


        public string EditCustSocial(int id, string facebook, string skypeId, string viberId)
        {
            try
            {
                CustSocialAcc social = db.CustSocialAccs.Find(id);
                social.Facebook = facebook;
                social.Skype = skypeId;
                social.Viber = viberId;

                db.Entry(social).State = EntityState.Modified;
                db.SaveChanges();

                return "OK : ";
            }
            catch (Exception ex)
            {
                return "Cannot Process adding new social details." + ex;
            }
        }
        #endregion

        #region Vehicles 
        [HttpPost]
        public bool AddCustomerVehicle(int vehicleModelId, string yearModel, string plateNo, string conduction, string engineNo, string chassisNo, string color, int customerId, int? custEntMainId, string remarks)
        {

            try
            {
                if (custEntMainId == null || custEntMainId == 0)
                    custEntMainId = GetCustomerCompany(customerId);

                Vehicle vehicle = new Vehicle();

                vehicle.VehicleModelId = vehicleModelId;
                vehicle.YearModel = yearModel;
                vehicle.PlateNo = plateNo;
                vehicle.Conduction = conduction;
                vehicle.EngineNo = engineNo;
                vehicle.ChassisNo = chassisNo;
                vehicle.Color = color;
                vehicle.CustomerId = customerId;
                vehicle.CustEntMainId = (int)custEntMainId;
                vehicle.Remarks = remarks;

                db.Vehicles.Add(vehicle);
                db.SaveChanges();

                return true;
            }
            catch
            {

                return false;
            }
        }

        [HttpGet]
        public JsonResult GetVehicleDetails(int? id)
        {
            if (id == null)
                return null;
            var vehicle = db.Vehicles.Where(v => v.Id == id).Select(v => new
            {
                v.VehicleModelId,
                v.VehicleModel.Make,
                v.VehicleModel.VehicleBrand.Brand,
                v.VehicleModel.VehicleTransmission.Type,
                v.YearModel,
                v.PlateNo,
                v.Conduction,
                v.EngineNo,
                v.ChassisNo,
                v.Color,
                v.CustomerId,
                v.CustEntMainId,
                v.Remarks,
                v.Id
            });

            return Json(vehicle);

        }

        [HttpPost]
        public bool EditCustomerVehicle(int Id, int vehicleModelId, string yearModel, string plateNo, string conduction, string engineNo, string chassisNo, string color, int customerId, int? custEntMainId, string remarks)
        {

            try
            {
                if (custEntMainId == null || custEntMainId == 0)
                    custEntMainId = GetCustomerCompany(customerId);


                Vehicle vehicle = db.Vehicles.Find(Id);

                if (vehicle == null)
                {
                    return false;
                }

                vehicle.VehicleModelId = vehicleModelId;
                vehicle.YearModel = yearModel;
                vehicle.PlateNo = plateNo;
                vehicle.Conduction = conduction;
                vehicle.EngineNo = engineNo;
                vehicle.ChassisNo = chassisNo;
                vehicle.Color = color;
                vehicle.CustomerId = customerId;
                vehicle.CustEntMainId = (int)custEntMainId;
                vehicle.Remarks = remarks;

                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public bool DeleteCustomerVehicle(int Id)
        {

            try
            {
                Vehicle vehicle = db.Vehicles.Find(Id);

                if (vehicle == null)
                    return false;

                db.Vehicles.Remove(vehicle);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult VehicleServices(int? id, int? customerId)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var vehicleServices = jvc.GetJobVehicleServices((int)id);

            //get vehicle details
            var vehicle = db.Vehicles.Find(id);
            string vehicleDetails = vehicle.VehicleModel.VehicleBrand.Brand + " " + vehicle.VehicleModel.Make + " " + vehicle.YearModel +
                " (" + vehicle.PlateNo + ")";
            ViewBag.VehicleDetails = vehicleDetails;
            ViewBag.CustomerId = customerId;
            ViewBag.Customer = vehicle.Customer.Name;
            ViewBag.Company = vehicle.CustEntMain.Name;

            return View(vehicleServices);
        }

        public int GetCustomerCompany(int? id)
        {
            if (id == null)
            {
                return 1;
            }

            var custEntities = db.CustEntities.Where(c => c.CustomerId == id);
            if (custEntities.FirstOrDefault() != null)
            {
                return custEntities.FirstOrDefault().CustEntMainId;
            }
            else
            {
                //find public company
                var defaultCompany = db.CustEntMains.Where(c => c.Name == "Public");

                //for personal use
                var personalUse = db.CustEntMains.Where(c => c.Name == "Personal Use");

                if (defaultCompany.FirstOrDefault() != null)
                {
                    return defaultCompany.FirstOrDefault().Id;
                }
                else if (personalUse.FirstOrDefault() != null)
                {
                    return personalUse.FirstOrDefault().Id;
                }
                else
                {
                    return 1;
                }
            }
        }

        [HttpGet]
        public JsonResult GetCustomerCompanyOrDefault(int? id)
        {
            var Id = 0;
            if (id == null)
            {
                Id = 1;
            }

            var custEntities = db.CustEntities.Where(c => c.CustomerId == id);
            if (custEntities.FirstOrDefault() != null)
            {
                Id = custEntities.FirstOrDefault().CustEntMainId;
            }
            else
            {
                //find public company
                var defaultCompany = db.CustEntMains.Where(c => c.Name == "Public");

                //for personal use
                var personalUse = db.CustEntMains.Where(c => c.Name == "Personal Account");

                if (defaultCompany.FirstOrDefault() != null)
                {
                    Id = defaultCompany.FirstOrDefault().Id;
                }
                else if (personalUse.FirstOrDefault() != null)
                {
                    Id = personalUse.FirstOrDefault().Id;
                }
                else
                {
                    Id = 1;
                }
            }

            return Json(Id);
        }

        #endregion
    }
}
