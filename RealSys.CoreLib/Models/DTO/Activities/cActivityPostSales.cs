using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Activities
{
    public class cActivityPostSales
    {
        public string SalesCode { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public string ActivityType { get; set; }
        public decimal Amount { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }
        public string AssignedTo { get; set; }
        public string Remarks { get; set; }
        public CustEntActPostSale ActPostSale { get; set; }
    }
}
