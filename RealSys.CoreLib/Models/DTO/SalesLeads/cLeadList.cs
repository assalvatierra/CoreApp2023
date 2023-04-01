using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.SalesLeads
{
    public class cLeadList
    {
        public int Id { get; set; }
        public int CustEntMainId { get; set; }
        public string CustEntMain { get; set; }
        public cSalesLead SalesLeads { get; set; }
        public ICollection<SalesStatus> SalesStatus { get; set; }
        public ICollection<SalesLeadCategory> SalesLeadCategories { get; set; }
        public ICollection<SalesActivity> SalesActivities { get; set; }
    }
}
