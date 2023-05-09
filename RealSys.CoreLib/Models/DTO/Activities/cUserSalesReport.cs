using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Activities
{
    public class cUserSalesReport
    {
        public string User { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalQuotation { get; set; }
        public decimal TotalProcurement { get; set; }
        public decimal TotalJobOrder { get; set; }
    }
}
