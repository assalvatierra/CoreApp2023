using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.SalesLeads
{
    public class cSalesLead : SalesLead
    {
        public int CustEntMainId { get; set; }
        public string Company { get; set; }

        public string ActivityStatus { get; set; }
        public string ActivityStatusType { get; set; }

        public int FileCount { get; set; }

        public ICollection<SalesProcStatus> SalesProcStatuses { get; set; }

    }
}
