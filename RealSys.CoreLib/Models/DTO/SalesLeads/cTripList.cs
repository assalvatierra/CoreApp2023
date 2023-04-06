using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.SalesLeads
{

    public class cTripList
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public int JobServicesId { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public string Unit { get; set; }
        public string Driver { get; set; }
        public string ItemCode { get; set; }
        public string ViewLabel { get; set; }
        public string Particulars { get; set; }
        public string Description { get; set; }
        public int JobStatusId { get; set; }
        public Nullable<Decimal> ActualAmt { get; set; }
        public string items { get; set; }

    }

}
