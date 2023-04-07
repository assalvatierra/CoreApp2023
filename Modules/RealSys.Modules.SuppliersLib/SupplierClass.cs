using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.DTO.Customers;
using RealSys.CoreLib.Models.DTO.Products;
using RealSys.CoreLib.Models.DTO.Suppliers;
using RealSys.CoreLib.Models.Erp;
using RealSys.Modules.SysLib.Lib;

namespace RealSys.Modules.SuppliersLib
{
    public class SupplierClass
    {


        private ErpDbContext db;
        private DateClass dt = new DateClass();
        private ILogger logger;

        public SupplierClass(ErpDbContext erpDb, ILogger _logger)
        {
            db = erpDb;
            logger = _logger;
            dt = new DateClass();
        }

        //get supplier list containing the search string,
        //if search is empty, return all actve items
        public List<cSupplierItems> getSupplierList(string search, string category, string status, string sort)
        {

            List<cSupplierList> custList = new List<cSupplierList>();

            //string sql ="SELECT * ," +
            //            "Country = (SELECT Name FROM Countries cty WHERE sup.CountryId = cty.Id )," +
            //            "City = (SELECT Name FROM Cities ct WHERE sup.CityID = ct.Id ),"+
            //            "SupType = (SELECT Description FROM SupplierTypes supt WHERE sup.SupplierTypeId = supt.Id )"+
            //            "FROM Suppliers sup ";

            //sql query with comma separated item list
            string sql = " SELECT * FROM (SELECT * ,  " +
                         "Country = (SELECT Name FROM Countries cty WHERE sup.CountryId = cty.Id ), " +
                         "City = (SELECT Name FROM Cities ct WHERE sup.CityID = ct.Id ), " +
                         "SupType = (SELECT Description FROM SupplierTypes supt WHERE sup.SupplierTypeId = supt.Id ), " +
                         "Items = SUBSTRING((SELECT(SELECT ii.Description as [text()] FROM InvItems ii WHERE sii.InvItemId = ii.Id FOR XML PATH('')) + ', ' " +
                         "FROM SupplierInvItems sii WHERE sup.Id = sii.SupplierId FOR XML PATH('')),1,100 ) " +
                         "FROM Suppliers sup ) as ItemList ";

            //handle status filter
            if (status != "ALL")
            {
                if (status == "ACT")
                {
                    sql += " WHERE (ItemList.Status = 'ACT' OR ItemList.Status = 'ACC' OR ItemList.Status = 'AOP') ";
                }
                else
                {
                    sql += " WHERE ItemList.Status = '" + status + "' ";
                }
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
                    sql += " AND ";
                }
                else
                {
                    sql += " WHERE ";
                }

                switch (category)
                {
                    case "COUNTRY":
                        sql += " ItemList.Country Like '%" + search + "%' ";
                        break;
                    case "CATEGORY":
                        sql += " ItemList.supType Like '%" + search + "%' ";
                        break;
                    case "SUPPLIER":
                        sql += " ItemList.Name Like '%" + search + "%' ";
                        break;
                    case "PRODUCT":
                        sql += " ItemList.Items Like '%" + search + "%' ";
                        break;
                    case "CITY":
                        sql += " ItemList.City Like '%" + search + "%' ";
                        break;
                    default:
                        sql += " (ItemList.Name Like '%" + search + "%' OR ItemList.Items Like '%" + search + "%')  ";
                        break;
                }
            }

            if (sort != null)
            {
                switch (sort)
                {
                    case "DATE":
                        sql += "ORDER BY Id ASC;";
                        break;
                    case "NAME":
                        sql += "ORDER BY Name ASC;";
                        break;
                    case "JOBSCOUNT":
                        sql += "ORDER BY JobCount DESC;";
                        break;
                    default:
                        sql += "ORDER BY Name ASC;";
                        break;
                }
            }
            else
            {
                //terminator
                sql += "ORDER BY Name ASC;";

            }

            // custList = db.Database.SqlQuery<cSupplierList>(sql).ToList();

            //custList = db.Database.ExecuteSqlRaw(sql);
            // var data = "";

            // custList = db.cSupplierLists.FromSqlRaw(sql).ToList();

            var suppliers = db.Suppliers
                .Include(s=>s.City)
                .Include(s => s.Country)
                .Include(s => s.SupplierType)
                .Include(s => s.SupplierContacts)
                .Include(s => s.SupplierInvItems)
                .Include(s => s.SupplierItems)
                .ToList();

            //return custList;
            List<cSupplierItems> supItems = new List<cSupplierItems>();

            foreach (var sup in suppliers)
            {
            //    //get products of the supplier
            //    var products = db.SupplierInvItems.Where(s => s.SupplierId == sup.Id).ToList();

             //   IEnumerable<string> contactsPerson = GetContactNames(sup.Id);
            //    IEnumerable<string> contactsDetails = GetContactDetails(sup.Id);

            //    //get product list with price
            //    List<cProduct> prods = new List<cProduct>();
            //    List<string> prodUnitPrices = new List<string>();


            //    foreach (var item in products)
            //    {
            //        if (item.SupplierItemRates.OrderByDescending(s => s.Id).FirstOrDefault() != null)
            //        {
            //            var price = item.SupplierItemRates.OrderByDescending(s => s.Id).FirstOrDefault();

            //            prods.Add(new cProduct
            //            {
            //                Id = item.InvItemId,
            //                ProductName = item.InvItem.Description,
            //                Price = price.ItemRate,
            //                UnitPrice = price.SupplierUnit.Unit

            //            });

            //            prodUnitPrices.Add(item.InvItem.Description
            //                + " <br> <span style='color:green;padding-bottom:10px;'> "
            //                + price.ItemRate + " / " + price.SupplierUnit.Unit + "</span> ");
            //        }
            //        else
            //        {

            //            prodUnitPrices.Add(item.InvItem.Description);
            //        }
            //    }


                supItems.Add(new cSupplierItems
                {
                    Id = sup.Id,
                    Name = sup.Name,
                    Code = sup.Code,
                    Country = sup.Country.Name,
                    City = sup.City.Name,
                    Category = sup.SupplierType.Description,
                    Status = sup.Status,
                    Product = new List<string>(),
                    ContactPerson = sup.SupplierContacts.Select(c=>c.Name),
                    ContactNumber = sup.SupplierContacts.Select(c=>c.Mobile)

                });
            }


            return supItems;
        }

        //get supplier list containing the search string,
        //if search is empty, return all actve items
        public List<cProductList> getProductList(string search, string category, string status, string sort)
        {

            List<cProductList> prodList = new List<cProductList>();

            //sql query with comma separated item list
            string sql =
                "SELECT * FROM (SELECT sii.Id, ii.Description as Name, Supplier = ( SELECT supp.Name FROM Suppliers supp WHERE sii.SupplierId = supp.Id )," +
                " sir.Particulars, sii.SupplierId, sir.ItemRate, sir.Material, su.Unit, sir.DtEntered, sir.DtValidFrom, sir.DtValidTo, sir.Remarks, sup.Status, sir.Origin, " +
                " IsValid = IIF(convert(datetime, sir.DtValidTo) < convert(datetime, GETDATE()), 1, 0) " +
                " FROM SupplierInvItems sii LEFT JOIN Suppliers sup ON sii.Id = sup.Id " +
                " LEFT JOIN SupplierItemRates sir on sii.Id = sir.SupplierInvItemId " +
                " LEFT JOIN InvItems ii ON sii.InvItemId = ii.Id " +
                " LEFT JOIN SupplierUnits su ON sir.SupplierUnitId = su.Id) as prods ";

            //get products 6 months before the validfrom
            sql += "WHERE (ISNULL(prods.ItemRate,'0') = '0') OR  convert(datetime, prods.DtValidFrom) > convert(datetime, DATEADD(DAY, -180, GETDATE())) ";

            //handle status filter
            if (status != "ALL")
            {
                sql += " AND prods.Status = '" + status + "' ";
            }

            //handle status filter
            if (status == "ALL")
            {
                sql += " ";
            }

            //handle search by name filter
            if (search != null || search != "")
            {


                switch (category)
                {
                    case "PRODUCT":
                        sql += " AND prods.Name Like '%" + search + "%'  ";
                        break;
                    case "ORIGIN":
                        sql += " AND prods.Origin Like '%" + search + "%' ";
                        break;
                    case "MATERIAL":
                        sql += " AND prods.Material Like '%" + search + "%' ";
                        break;
                    default:
                        sql += " AND prods.Name Like '%" + search + "%'  ";
                        break;
                }
            }

            if (sort != null)
            {
                switch (sort)
                {
                    case "LATEST-DATE":
                        sql += " ORDER BY IsValid ASC, convert(datetime, DtValidFrom)  DESC;";
                        break;
                    case "LOWEST-DATE":
                        sql += " ORDER BY prods.DtEntered ASC;";
                        break;
                    case "LOWEST-PRICE":
                        sql += " ORDER BY prods.ItemRate ASC;";
                        break;
                    default:
                        sql += " ORDER BY prods.Name ASC;";
                        break;
                }
            }
            else
            {
                //terminator
                sql += " ORDER BY prods.Name ASC;";

            }

            //prodList = db.Database.SqlQuery<cProductList>(sql).ToList();

            prodList = db.cProductLists.FromSqlRaw(sql).ToList();

            //get list of products of 
            var productNoRate = db.SupplierInvItems.Where(s => s.SupplierItemRates.Count == 0 && s.InvItem.Description.Contains(search)).ToList();
            foreach (var prod in productNoRate)
            {

                prodList.Add(new cProductList
                {
                    Id = prod.Id,
                    Name = prod.InvItem.Description,
                    Supplier = prod.Supplier.Name,
                    SupplierId = prod.Supplier.Id,
                    ItemRate = "No Rate",
                    IsValid = 0
                });

            }

            return prodList;
        }




        //get supplier list containing the search string,
        //if search is empty, return all actve items
        public List<SupplierInvItem> getSuppInvRate(int id)
        {

            List<cSupplierItem> prodList = new List<cSupplierItem>();

            //sql query with comma separated item list
            string sql =
               @"
                 SELECT sii.* , sir.DtValidFrom FROM SupplierInvItems sii
                 LEFT JOIN SupplierItemRates sir on sir.SupplierInvItemId = sii.Id
                 WHERE sii.SupplierId = " + id + " ORDER BY sir.DtValidFrom DESC ";

            //prodList = db.Database.SqlQuery<cSupplierItem>(sql).ToList();

            prodList = db.cSupplierItems.FromSqlRaw(sql).ToList();

            List<int> IdList = prodList.Select(s => s.Id).ToList();

            List<SupplierInvItem> supItems = db.SupplierInvItems.Where(s => IdList.Contains(s.Id)).ToList();

            return supItems;
        }


        //GET contact persons of existing suppliers and return as list 
        private IEnumerable<cContactPerson> GetContactPersons(int supplierId)
        {
            List<cContactPerson> contactList = new List<cContactPerson>();

            var contacts = db.SupplierContacts.Where(s => s.SupplierId == supplierId).ToList();

            foreach (var person in contacts)
            {
                contactList.Add(new cContactPerson
                {
                    Id = person.Id,
                    Name = person.Name,
                    Number = person.Mobile,
                    Email = person.Email
                });
            }

            return contactList;
        }

        private List<String> GetContactNames(int supplierId)
        {
            List<String> nameList = new List<string>();

            var contactsPerson = GetContactPersons(supplierId);
            foreach (var person in contactsPerson)
            {
                nameList.Add(person.Name);
            }

            return nameList;
        }

        private List<String> GetContactDetails(int supplierId)
        {
            List<String> detailList = new List<string>();

            var contactsPerson = GetContactPersons(supplierId);
            foreach (var person in contactsPerson)
            {
                var contactDetails = person.Number + " / " + person.Email;
                detailList.Add(contactDetails);
            }

            return detailList;
        }


        //Customers on the list are customers with
        //a year past jobs and no recent jobs 
        public List<Supplier> getDeactivatedList()
        {

            List<int> oldJobs = new List<int>();
            var datetoday = dt.GetCurrentDate().Date.AddDays(-360).Date;

            //get all active suppliers
            var actSuppliers = db.Suppliers.Where(s => s.Status == "ACT").ToList();

            foreach (var sup in actSuppliers)
            {
                //get recent jobservice of supplier
                var jobserviceTemp = db.JobServices.Where(j => j.SupplierId == sup.Id).OrderByDescending(s => s.DtStart).FirstOrDefault();
                if (jobserviceTemp != null)
                {
                    DateTime jobdate = (DateTime)jobserviceTemp.DtStart;
                    //check if job is old o more than a year
                    if (jobdate.Date.CompareTo(datetoday.Date) < 0)
                    {
                        oldJobs.Add(jobserviceTemp.Id);
                    }

                }
            }

            //get recent of jobservices 360 days
            var services = db.JobServices.Where(j => oldJobs.Contains(j.Id)).ToList().Select(s => s.SupplierId);

            //get list of suppliers with jobs
            var suppliers = db.Suppliers.Where(s => services.Contains(s.Id)).ToList();

            return suppliers;
        }


        public string removeEmailString(string input)
        {
            try
            {
                char ch = '@';
                int idx = input.IndexOf(ch);
                return input.Substring(0, idx);
            }
            catch
            {
                return input;
            }
        }

    }
}