using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Activities
{
    public class cActivitySearchResult
    {
        public List<CustEntActivity> CustEntActivities { get; set; }
        public List<SupplierActivity> SupplierActivities { get; set; }
    }
}
