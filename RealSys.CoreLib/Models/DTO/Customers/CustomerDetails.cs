using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Customers
{ 
    public class CustomerDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public int JobID { get; set; }
        public int CustCategoryID { get; set; }
        public string CustCategoryIcon { get; set; }
        public int CustEntID { get; set; }
        public string CustEntName { get; set; }
        public string CustEntIconPath { get; set; }
        public List<CustCategory> categories { get; set; }
        public string Company { get; set; }
    }
}
