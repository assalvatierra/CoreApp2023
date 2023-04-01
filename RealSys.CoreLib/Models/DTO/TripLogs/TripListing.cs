using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.TripLogs
{
    public class TripListing
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public int JobServicesId { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public DateTime DtService { get; set; }
        public List<string> Unit { get; set; }
        public List<string> Driver { get; set; }
        public string ItemCode { get; set; }
        public string ViewLabel { get; set; }
        public string Particulars { get; set; }
        public string Description { get; set; }
        public string JobStatus { get; set; }
        public Nullable<Decimal> ActualAmt { get; set; }
        public Decimal Fuel { get; set; }
        public Decimal DriverComi { get; set; }
        public Decimal OperatorComi { get; set; }
        public Decimal Payment { get; set; }
        public string items { get; set; }
        public bool? DriverForRelease { get; set; }
        public bool? DriverIsReleased { get; set; }
        public bool? OperatorForRelease { get; set; }
        public bool? OperatorIsReleased { get; set; }

    }
}
