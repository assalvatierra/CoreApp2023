using RealSys.Modules.CustomersLib.Custom;
using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealSys.Modules.SysLib.Lib;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RealSys.Modules.CustomersLib.Lib
{
    public class CustomerClass
    {

        private ErpDbContext db;
        private DateClass dt;

        public CustomerClass(ErpDbContext dbContext) {
            db = dbContext;
            dt = new DateClass();
        }

        //get categories of the customers by Id on Partial View
        public List<CustCategory> getCategoriesList(int id)
        {

            //PartialView for Details of the Customer
            List<CustCategory> categoryDetails = new List<CustCategory>();

            //error
            var categoryList = db.CustCats.Where(c => c.CustomerId == id).ToList();

            if (categoryList == null)
            {
                categoryDetails.Add(new CustCategory
                {
                    Id = 0,
                    iconPath = "Images/Customers/Category/unavailable-40.png",
                    Name = "not assigned"
                });

            }
            else
            {

                foreach (var cat in categoryList)
                {
                    switch (cat.CustCategory.Name)
                    {
                        case "PRIORITY":
                            cat.CustCategory.iconPath = "Images/Customers/Category/star-filled-40.png";
                            break;
                        case "ACTIVE":
                            cat.CustCategory.iconPath = "Images/Customers/Category/Active-30.png";
                            break;
                        case "SUSPENDED":
                            cat.CustCategory.iconPath = "Images/Customers/Category/suspended-64.png";
                            break;
                        case "BAD ACCOUNT":
                            cat.CustCategory.iconPath = "Images/Customers/Category/cancel-40.png";
                            break;
                        case "CAR-RENTAL":
                            cat.CustCategory.iconPath = "Images/Customers/Category/car-40.png";
                            break;
                        case "TOUR":
                            cat.CustCategory.iconPath = "Images/Customers/Category/tour-40.png";
                            break;
                        case "CLIENT":
                            cat.CustCategory.iconPath = "Images/Customers/Category/client-40.png";
                            break;
                        case "COMPANY":
                            cat.CustCategory.iconPath = "Images/Customers/Company/organization-40.png";
                            break;
                    }

                    categoryDetails.Add(new CustCategory
                    {
                        Id = cat.CustCategory.Id,
                        iconPath = cat.CustCategory.iconPath,
                        Name = cat.CustCategory.Name

                    });
                }
            }

            return categoryDetails.OrderByDescending(s => s.Name).ToList();
        }

        //get companies associated to the customer using 
        //the customer id, then return the list of companies to the controller
        public List<CustEntMain> getCustCompanies(int id)
        {
            List<CustEntMain> CompanyList = new List<CustEntMain>();
            //error
            var CompanyRecord = db.CustEntities
                .Include(c=>c.CustEntMain)
                .Where(c => c.CustomerId == id)
                .OrderByDescending(s => s.Id)
                .ToList();

            if (CompanyRecord == null)
            {
                CompanyList.Add(new CustEntMain
                {
                    Id = 0,
                    Name = "None",
                    Address = "None",
                    Contact1 = "None",
                    Contact2 = "None",
                    iconPath = "None"
                });
            }
            else
            {
                foreach (var record in CompanyRecord)
                {
                    CompanyList.Add(new CustEntMain
                    {
                        Id = record.CustEntMain.Id,
                        Name = record.CustEntMain.Name,
                        Address = record.CustEntMain.Address,
                        Contact1 = record.CustEntMain.Contact1,
                        Contact2 = record.CustEntMain.Contact2,
                        iconPath = record.CustEntMain.iconPath
                    });

                }
            }
            return CompanyList;
        }

        //get files of the customers by Id  on Partial View
        public List<CustFiles> getCustFiles(int custId)
        {
            List<CustFiles> FilesList = new List<CustFiles>();
            List<CustFiles> customerFiles;
            //error
            try
            {
                customerFiles = db.CustFiles.Where(c => c.CustomerId == custId).ToList();

                //handle empty customer files
                if (customerFiles == null)
                {
                    FilesList.Add(new CustFiles
                    {
                        Id = 0,
                        CustomerId = 0,
                        Path = "none",
                        Desc = "none",
                        Folder = "none",
                        Remarks = ""
                    });
                }
                else
                {
                    //add customer files to the list 
                    foreach (var file in customerFiles)
                    {
                        FilesList.Add(new CustFiles
                        {
                            Id = file.Id,
                            CustomerId = file.CustomerId,
                            Path = file.Path,
                            Desc = file.Desc,
                            Folder = file.Folder,
                            Remarks = file.Remarks,

                        });
                    }
                }
            }
            catch { }

            return FilesList;
        }


        public void CreateSocialAccount(int custId, string account)
        {
            try
            {

                CustSocialAcc social = new CustSocialAcc();
                social.CustomerId = custId;
                social.Facebook = account;
                social.Skype = "";
                social.Viber = "";

                db.CustSocialAccs.Add(social);
                db.SaveChanges();

            }
            catch { }
        }

        //get customer jobs on Partial View
        #region Customer jobs
        //get customer jobs and details
        public List<CustomerJobDetails> getCustomerJobList(int id, int top, string sdate, string edate, string status, string sortdate)
        {
            int topFilter = (int)top;
            //PartialView for Details of the Customer
            List<CustomerJobDetails> jobList = new List<CustomerJobDetails>();

            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;

            //handle empty dates
            if (sdate != null && edate != null)
            {
                StartDate = DateTime.Parse(sdate).Date;
                EndDate = DateTime.Parse(edate).Date;
            }

            //error
            var jobRecord = db.JobMains.Where(j => j.CustomerId == id).ToList();

            if (jobRecord != null)
            {
                if (!(String.IsNullOrEmpty(sortdate)))
                {
                    if (sortdate == "1")
                    {
                        jobRecord = jobRecord.OrderByDescending(j => j.JobDate).ToList();
                    }
                    else
                    {
                        jobRecord = jobRecord.OrderBy(j => j.JobDate).ToList();
                    }
                }

                //handle empty status
                if (status == null || status == "" || status == "ALL")
                {
                    jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0).ToList();
                }
                else
                {
                    jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0 && j.JobStatus.Status == status).ToList();
                }
            }

            //get customised jobList
            jobList = getJobList(jobRecord);

            return jobList.Take(topFilter).ToList();
        }

        //get list of Customer Job details and return it
        private List<CustomerJobDetails> getJobList(List<JobMain> jobRecord)
        {
            List<CustomerJobDetails> jobList = new List<CustomerJobDetails>();

            if (jobRecord == null)
            {
                jobList.Add(new CustomerJobDetails
                {
                    Id = 0,
                    JobDate = "NA",
                    Description = "none",
                    AgreedAmt = "0",
                    NoOfDays = "0",
                    NoOfPax = "0",
                    StatusRemarks = "none",
                    PaymentStatus = "NA",
                });
            }
            else
            {
                foreach (var record in jobRecord)
                {
                    var svcs = db.JobServices.Where(s => s.JobMainId == record.Id).ToList();
                    decimal totalAmount = 0;

                    foreach (var services in svcs)
                    {
                        totalAmount += services.ActualAmt != null ? (decimal)services.ActualAmt : 0;

                        jobList.Add(new CustomerJobDetails
                        {
                            Id = record.Id,
                            JobDate = record.JobDate.ToShortDateString(),
                            Description = record.Description,
                            JobSvcDates = ((DateTime)services.DtStart).ToShortDateString() + " - " + ((DateTime)services.DtEnd).ToShortDateString(),
                            SvcParticulars = services.Particulars,
                            SvcType = services.Service.Name,
                            AgreedAmt = record.AgreedAmt.ToString(),
                            NoOfDays = record.NoOfDays.ToString(),
                            NoOfPax = record.NoOfPax.ToString(),
                            StatusRemarks = record.JobStatus.Status,
                            Amount = (Decimal)services.ActualAmt,
                            PaymentStatus = GetLastJobPaymentStatus(record.Id)
                        });
                    }
                }
            }
            return jobList;
        }


        private string GetLastJobPaymentStatus(int jobId)
        {
            var tempStatus = db.JobMainPaymentStatus.Where(j => j.JobMainId == jobId);

            if (tempStatus.FirstOrDefault() != null)
            {
                return tempStatus.OrderByDescending(j => j.Id).FirstOrDefault().JobPaymentStatu.Status;
            }

            //unpaid if no records
            return "Unpaid";
        }


        #endregion

        //old table generated by through mvc
        #region old_customer_Table

        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        public List<CustomerDetails> getCustomerList(string status, string search)
        {

            var customerList = new List<Customer>();

            //filter customer list with status
            //customerList = filterCustomerStatus(status);

            if (!String.IsNullOrEmpty(search) && !String.IsNullOrWhiteSpace(search))
            {
                //filter customer list with search string
                customerList = searchCustomer(customerList, search);
            }

            List<CustomerDetails> customerDetailList = new List<CustomerDetails>();
            foreach (var customer in customerList)
            {
                CustCategory custcategory = new CustCategory();
                CustCat custcat = new CustCat();
                CustEntity companyEntity = new CustEntity();
                CustEntMain company = new CustEntMain();

                //try
                //{
                //    custcat = db.CustCats.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                //    custcategory = db.CustCategories.Where(cat => cat.Id == custcat.CustCategoryId).FirstOrDefault();

                //}
                //catch
                //{
                //    custcategory = new CustCategory
                //    {
                //        Id = 0,
                //        Name = "Not Assigned",
                //        iconPath = "Images/Customers/Category/unavailable-40.png"
                //    };
                //}

                //try
                //{
                //    companyEntity = db.CustEntities.Where(ce => ce.CustomerId == customer.Id).FirstOrDefault();
                //    company = db.CustEntMains.Where(co => co.Id == companyEntity.CustEntMainId).FirstOrDefault();

                //}
                //catch
                //{
                //    company = new CustEntMain
                //    {
                //        Id = 0,
                //        Name = "Not Assigned",
                //        Address = "none",
                //        Contact1 = "0",
                //        Contact2 = "0",
                //        iconPath = "Images/Customers/Category/unavailable-40.png"
                //    };
                //}


                customerDetailList.Add(new CustomerDetails
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Contact1 = customer.Contact1,
                    Contact2 = customer.Contact2,
                    Remarks = customer.Remarks,
                    Status = customer.Status,
                    //JobID = customer.JobMains.Count(),
                    //CustCategoryID = custcategory.Id,
                    //CustCategoryIcon = custcategory.iconPath,
                    //CustEntID = company.Id,
                    //CustEntName = company.Name,
                    //CustEntIconPath = "~/Images/Customers/Company/organization-40.png",
                    //categories = getCategoriesList(customer.Id),
                    //companies = getCustCompanies(customer.Id)

                    //end
                });

            }

            return customerDetailList;
        }

        private List<Customer> filterCustomerStatus(string status)
        {

            var customerList = new List<Customer>();

            switch (status)
            {
                case "ACTIVE":
                    customerList = db.Customers.Where(s => s.Status == "ACT").ToList();
                    break;
                case "INACTIVE":
                    customerList = db.Customers.Where(s => s.Status == "INC").ToList();
                    break;
                case "BAD":
                    customerList = db.Customers.Where(s => s.Status == "BAD").ToList();
                    break;
                case "ALL":
                    customerList = db.Customers.ToList();
                    break;
                default:
                    customerList = db.Customers.Where(s => s.Status == "ACT").ToList();
                    break;
            }


            return customerList;
        }

        private List<Customer> searchCustomer(List<Customer> customerList, string search)
        {
            customerList = customerList.Where(c => c.Name.ToLower().Contains(search.ToLower())).ToList();

            return customerList;
        }

        #endregion

        //new table through ajax call
        #region AJAX_Customer_Table
        //-----AJAX Functions for generating table list---------//

        //GET : Customer List
        public List<CustomerList> GetCustomerList(string search, string status, string sort, string user)
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerList> custList = new List<CustomerList>();

            string sql = "SELECT c.Id, c.Name, c.Contact1, c.Contact2, c.Status,"
                        + " JobCount = (SELECT Count(x.Id) FROM [JobMains] x WHERE x.CustomerId = c.Id ) ,"
                        + " Company = (SELECT Top(1)  CompanyName = (SELECT Top(1) cem.Name FROM [CustEntMains] cem where ce.CustEntMainId = cem.Id ORDER BY cem.Id DESC)"
                        + " FROM [CustEntities] ce WHERE ce.CustomerId = c.Id  ORDER BY ce.Id DESC) FROM Customers c"
                        + " INNER JOIN CustEntities cen ON cen.CustomerId = c.Id"
                        + " LEFT JOIN CustEntMains cem ON cem.Id = cen.CustEntMainId"
                        + " WHERE cem.Exclusive = 'PUBLIC' OR ISNULL(cem.Exclusive,'PUBLIC') = 'PUBLIC' OR (cem.Exclusive = 'EXCLUSIVE' AND cem.AssignedTo = '" + user + "') "
                        ;

            //handle status filter
            if (status != "ALL")
            {
                sql += " AND c.Status = '" + status + "' ";
            }

            //handle status filter
            if (status == "ALL")
            {
                sql += " ";
            }

            //handle search by name filter
            if (search != null || search != "")
            {
                //handle status filter
                if (status != "ALL")
                {
                    sql += " AND c.Name Like '%" + search + "%' ";
                }
                else
                {
                    sql += " AND c.Name Like '%" + search + "%' ";
                }
            }

            if (sort != null)
            {
                switch (sort)
                {
                    case "DATE":
                        sql += " ORDER BY Id ASC;";
                        break;
                    case "NAME":
                        sql += " ORDER BY Name ASC;";
                        break;
                    case "JOBSCOUNT":
                        sql += " ORDER BY JobCount DESC;";
                        break;
                    default:
                        sql += " ORDER BY JobCount DESC , Name ASC;";
                        break;
                }
            }
            else
            {
                //terminator
                sql += " ORDER BY JobCount ASC , Name ASC;";

            }

            var sqlCustomers = db.Customers.FromSqlRaw(sql).ToList();

            if (sqlCustomers != null)
            {
                foreach (var cust in sqlCustomers)
                {
                    custList.Add(new CustomerList
                    {
                        Id = cust.Id,
                        Company = cust.CustEntities.First().Company,
                        Contact1 = cust.Contact1,
                        Contact2 = cust.Contact2,
                        JobCount = cust.JobMains.Count,
                        Name = cust.Name,
                        Status = cust.Status
                    });
                }
            }
             
            return custList;
        }

        //GET : Customer Admin List
        public List<CustomerList> GetCustomerAdminList(string search, string status, string sort)
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerList> custList = new List<CustomerList>();

            string sql = "SELECT c.Id, c.Name, c.Contact1, c.Contact2 , c.Status, c.Email, c.Remarks, "
                        + " JobCount = (SELECT Count(x.Id) FROM [JobMains] x WHERE x.CustomerId = c.Id ) ,"
                        + " Company = (SELECT Top(1)  CompanyName = (SELECT Top(1) cem.Name FROM [CustEntMains] cem where ce.CustEntMainId = cem.Id ORDER BY cem.Id DESC)"
                        + " FROM [CustEntities] ce WHERE ce.CustomerId = c.Id  ORDER BY ce.Id DESC) FROM Customers c "
                        ;

            //handle status filter
            if (status != "ALL")
            {
                sql += " WHERE c.Status = '" + status + "' ";
            }

            //handle status filter
            if (status == "ALL")
            {
                sql += " ";
            }

            //handle search by name filter
            if (search != null || search != "")
            {
                //handle status filter
                if (status != "ALL")
                {
                    sql += " AND  c.Name Like '%" + search + "%' ";
                }
                else
                {
                    sql += " WHERE  c.Name Like '%" + search + "%' ";
                }
            }

            if (sort != null)
            {
                switch (sort)
                {
                    case "DATE":
                        sql += " ORDER BY Id ASC;";
                        break;
                    case "NAME":
                        sql += " ORDER BY Name ASC;";
                        break;
                    case "JOBSCOUNT":
                        sql += " ORDER BY JobCount DESC;";
                        break;
                    default:
                        sql += " ORDER BY JobCount DESC , Name ASC;";
                        break;
                }
            }
            else
            {
                //terminator
                sql += " ORDER BY JobCount ASC , Name ASC;";

            }


            var sqlCustomers = db.Customers.FromSqlRaw(sql).ToList();

            if (sqlCustomers != null)
            {
                foreach (var cust in sqlCustomers)
                {
                    custList.Add(new CustomerList
                    {
                        Id = cust.Id,
                        //Company = this.getCustCompanyName(cust.Id),
                        Contact1 = cust.Contact1,
                        Contact2 = cust.Contact2,
                        JobCount = cust.JobMains.Count,
                        Name = cust.Name,
                        Status = cust.Status
                    });
                }
            }


            return custList;
        }


        //Get the latest company name of the customer
        private string getCustCompanyName(int custID)
        {
            string companyName = "";
            try
            {
                var custEnt = db.CustEntities.Where(s => s.CustomerId == custID).OrderByDescending(s => s.Id).FirstOrDefault();

                if (custEnt != null)
                {
                    if (custEnt.CustEntMain != null)
                    {

                        companyName = custEnt.CustEntMain.Name ;
                    }
                }

               
            }
            catch
            { }

            return companyName;

        }


        //Get the number of jobs of the customer
        private string getjobCount(int custID)
        {
            return db.Customers.Find(custID).JobMains.Count().ToString();

        }
        #endregion

        //decatvate customers
        #region Deactivate Operations

        //Customers on the list are customers with
        //a year past jobs and no recent jobs 
        public List<Customer> getDeactivateCustomers()
        {
            List<int> latestJobs = new List<int>();

            //adjust date by subtracting 360 days (a year)
            var datetoday = dt.GetCurrentDate().Date.AddDays(-360);

            //get customers with jobs 
            var customersWithJobs = db.JobMains.Where(j => j.CustomerId > 0).Select(s => s.CustomerId);

            //get customers with null or ACT status and have jobs 
            var customers = db.Customers.Where(s => (string.IsNullOrEmpty(s.Status) || s.Status == "ACT") && customersWithJobs.Contains(s.Id)).ToList().Select(s => s.Id);

            //check group of customers and its recent job 
            //if the job is more than a year old
            foreach (var cust in customers)
            {
                JobMain tempjob = db.JobMains.Where(j => j.Customer.Id == cust).OrderByDescending(j => j.JobDate).FirstOrDefault();
                if (tempjob.JobDate.CompareTo(datetoday) < 0)
                {
                    latestJobs.Add(cust);
                }
            }

            //get customers with status NO STATUS or ACTIVE from prev list
            var CustomersList = db.Customers.Where(s => (string.IsNullOrEmpty(s.Status) || s.Status == "ACT") && latestJobs.Contains(s.Id)).ToList();

            //get customers  not in the list of customers with jobs
            List<Customer> noJobCustomer = db.Customers.Where(s => (string.IsNullOrEmpty(s.Status) || s.Status == "ACT") && s.Id != 1 && !customersWithJobs.Contains(s.Id)).ToList();

            //merge two list
            CustomersList.AddRange(noJobCustomer);

            return CustomersList;
        }

        #endregion

    }
}
