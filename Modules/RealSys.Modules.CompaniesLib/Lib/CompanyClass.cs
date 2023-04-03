using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.DTO.Companies;
using RealSys.CoreLib.Models.Erp;
using RealSys.Modules.SysLib.Lib;

namespace RealSys.Modules.CompaniesLib.Lib
{
    public class CompanyClass
    {

        private ErpDbContext db;
        private DateClass dt;

        public CompanyClass(ErpDbContext dbContext, ILogger logger)
        {
            db = dbContext;
            dt = new DateClass();
        }


        //new table through ajax call
        #region AJAX_Customer_Table

        //-----AJAX Functions for generating table list---------//
        public List<cCompanyList> generateCompanyList(string search, string searchCat, string status, string sort, string user)
        {
            try
            {

                List<CompanyList> custList = new List<CompanyList>();

                string sql = "SELECT * FROM (SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) " +
                     "FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), " +
                     "City = (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), " +
                     "cust.Name as ContactName, cust.Email as ContactEmail, cust.Contact1 as ContactNumber, " +
                     "cet.Position as ContactPosition " +
                     "FROM CustEntMains cem " +
                     "LEFT JOIN CustEntities cet ON cet.CustEntMainId = cem.Id " +
                     "LEFT JOIN Customers cust ON cust.Id = cet.CustomerId ) as com ";

                if (user == "admin")
                {
                    sql += " WHERE (Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE'))";
                }
                else
                {
                    sql += " WHERE (Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE' AND AssignedTo='" + user + "'))";
                }
                //"WHERE (Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE')) ";

                if (status != null)
                {

                    if (status == "ALL")
                    {

                    }
                    else
                    {
                        sql += " AND com.Status = '" + status + "' ";
                    }

                }
                else
                {
                    //status is null
                    sql += " AND com.Status != 'INC' OR com.Status != 'BAD' ";
                }


                //handle search by name filter
                if (search != null || search != "")
                {
                    sql += " AND ";
                    //search using the search by category
                    switch (searchCat)
                    {
                        case "Company":
                            sql += " com.Name Like '%" + search + "%' ";
                            break;
                        case "City":
                            sql += " com.City Like '%" + search + "%' ";
                            break;
                        case "ContactName":
                            sql += " com.ContactName Like '%" + search + "%' ";
                            break;
                        case "Category":
                            sql += " com.Category Like '%" + search + "%' ";
                            break;
                        case "AssignedTo":
                            sql += " com.AssignedTo Like '%" + search + "%' ";
                            break;
                        default:
                            sql += " ";
                            break;
                    }
                }

                if (sort != null)
                {
                    switch (sort)
                    {
                        //add more options for sorting
                        case "NAME":
                            sql += " ORDER BY com.Name ASC;";
                            break;
                        case "CITY":
                            sql += " ORDER BY com.City ASC;";
                            break;
                        case "CATEGORY":
                            sql += " ORDER BY com.Category ASC;";
                            break;
                        case "STATUS":
                            sql += " ORDER BY " +
                                " CASE com.Status" +
                                "   WHEN 'PRI' THEN 1" +
                                "   WHEN 'ACT' THEN 2" +
                                "   WHEN 'ACC' THEN 3" +
                                "   WHEN 'ACP' THEN 4" +
                                "   WHEN 'BIL' THEN 5" +
                                "   ELSE 6 " +
                                " END;";
                            break;
                        default:
                            sql += " ORDER BY com.Name ASC;";
                            break;
                    }
                }
                else
                {
                    //terminator
                    sql += " ORDER BY com.Name ASC;";
                }

                //Old .net framework not working
                //custList = db.Database.SqlQuery<CompanyList>(sql).ToList();

                //new .net 6 
                var sqlCompanies = db.CustEntMains.FromSqlRaw(sql).ToList();

                if (sqlCompanies != null)
                {
                    foreach (var company in sqlCompanies)
                    {
                        custList.Add(new CompanyList
                        {
                           Id = company.Id,
                           Name = company.Name,
                           Status = company.Status,
                           Code = company.Code,
                           Address = company.Address,
                           Category = "",
                           Exclusive = company.Exclusive,
                           Mobile = company.Mobile,
                           Remarks = company.Remarks,
                           Website = company.Website,
                           AssignedTo = company.AssignedTo

                        });
                    }
                }


                return getCompanyList(custList, user);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //-----AJAX Functions for generating table list---------//
        public List<cCompanyList> generateCompanyAdminList(string search, string searchCat, string status, string sort)
        {
            List<CompanyList> custList = new List<CompanyList>();

            string sql = "SELECT * FROM (SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), " +
                 "City = (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), " +

                 "cust.Name as ContactName, cust.Email as ContactEmail, cust.Contact1 as ContactNumber, " +
                 "cet.Position as ContactPosition " +

                 "FROM CustEntMains cem " +

                 "LEFT JOIN CustEntities cet ON cet.CustEntMainId = cem.Id " +

                 "LEFT JOIN Customers cust ON cust.Id = cet.CustomerId " +

                 ") as com ";


            if (status != null)
            {

                if (status == "ALL")
                {

                }
                else
                {
                    sql += " WHERE com.Status = '" + status + "' ";
                }

            }
            else
            {
                //status is null
                sql += " WHERE com.Status != 'INC' OR com.Status != 'BAD' ";
            }


            //handle search by name filter
            if (search != null || search != "")
            {

                if (status == "ALL")
                {
                    sql += " WHERE ";
                }
                else
                {
                    sql += " AND ";
                }

                //search using the search by category
                switch (searchCat)
                {
                    case "Company":
                        sql += " com.Name Like '%" + search + "%' ";
                        break;
                    case "City":
                        sql += " com.City Like '%" + search + "%' ";
                        break;
                    case "ContactName":
                        sql += " com.ContactName Like '%" + search + "%' ";
                        break;
                    case "Category":
                        sql += " com.Category Like '%" + search + "%' ";
                        break;
                    case "AssignedTo":
                        sql += " com.AssignedTo Like '%" + search + "%' ";
                        break;
                    default:
                        sql += " ";
                        break;
                }
            }


            if (sort != null)
            {
                switch (sort)
                {
                    //add more options for sorting
                    default:
                        sql += " ORDER BY com.Name ASC;";
                        break;
                }
            }
            else
            {
                //terminator
                sql += " ORDER BY com.Name ASC;";

            }

            //custList = db.Database.SqlQuery<CompanyList>(sql).ToList();

            //new .net framework
            var sqlCompanies = db.CustEntMains.FromSqlRaw(sql).ToList();

            if (sqlCompanies != null)
            {
                foreach (var company in sqlCompanies)
                {
                    custList.Add(new CompanyList
                    {
                        Id = company.Id,
                        Name = company.Name,
                        Status = company.Status,
                        Code = company.Code,
                        Address = company.Address,
                        Category = "",
                        Exclusive = company.Exclusive,
                        Mobile = company.Mobile,
                        Remarks = company.Remarks,
                        Website = company.Website,
                        AssignedTo = company.AssignedTo

                    });
                }
            }


            return getCompanyList(custList, "admin");
        }

        //Add Contact Persons to company list result
        private List<cCompanyList> getCompanyList(List<CompanyList> list, string user)
        {
            List<cCompanyList> comlist = new List<cCompanyList>();

            var prevId = 0;
            foreach (var com in list)
            {
                if (prevId == com.Id)
                {
                    continue;
                }

                //build contact list
                var contacts = db.CustEntities.Where(s => s.CustEntMainId == com.Id).ToList();
                var custEnts = db.CustEntities
                                .Include(c=>c.Customer)
                                .Include(c => c.CustEntMain)
                                .Where(s => s.CustEntMainId == com.Id).ToList();
                List<string> contactNames = new List<string>();
                List<string> contactPositions = new List<string>();
                List<string> contactNumberEmail = new List<string>();
                var isAssigned = false;

                if (user == com.AssignedTo || user == "admin")
                {
                    isAssigned = true;
                }

                //show contact details to admin and public
                if (isAssigned || com.Exclusive == "PUBLIC")
                {
                    contactNames = custEnts.Select(s => s.Customer.Name).ToList();
                    contactPositions = custEnts.Select(s => s.Position).ToList();

                    foreach (var items in custEnts)
                    {
                        var temp = "";
                        if (items.Customer.Contact2 != null)
                        {
                            temp = items.Customer.Contact1 + " | " + items.Customer.Contact2;
                        }
                        else
                        {
                            temp = items.Customer.Contact1;
                        }
                        contactNumberEmail.Add(temp + " <br> " + items.Customer.Email);
                    }
                }

                comlist.Add(new cCompanyList
                {
                    Id = com.Id,
                    Address = com.Address,
                    AssignedTo = removeSpecialChar(com.AssignedTo),
                    Category = com.Category,
                    City = com.City,
                    Code = com.Code,
                    Mobile = com.Mobile,
                    Name = com.Name,
                    Remarks = com.Remarks,
                    Status = com.Status,
                    Website = com.Website,
                    ContactName = contactNames,
                    ContactPosition = contactPositions,
                    ContactMobileEmail = contactNumberEmail,
                    Exclusive = com.Exclusive,
                    IsAssigned = isAssigned
                });

                prevId = com.Id;
            }

            return comlist;
        }

        public string removeSpecialChar(string input)
        {
            try
            {
                if (input == null)
                {
                    return "";
                }


                char ch = '@';
                int idx = input.IndexOf(ch);
                return input.Substring(0, idx);

            }
            catch
            {
                return input;
            }

        }

        #endregion


    }
}