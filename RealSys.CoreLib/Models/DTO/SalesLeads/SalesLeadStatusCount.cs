using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.SalesLeads
{
    public class SalesLeadStatusCount
    {
        public int Id {  get; set; }

        public int New { get; set; }
        public int Sales { get; set; }
        public int ForApproval { get; set; }
        public int Procurement { get; set; }
        public int Approved { get; set; }
        public int Awarded { get; set; }
        public int Closed { get; set; }
        public int Rejected { get; set; }
    }
}
