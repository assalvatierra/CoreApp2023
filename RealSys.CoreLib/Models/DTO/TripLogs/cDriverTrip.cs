using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.TripLogs
{

    public class cDriverTrip
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int JobMainId { get; set; }
        public int JobServiceId { get; set; }
        public string Name { get; set; }
        public string ItemCode { get; set; }
        public string Particulars { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public DateTime DtExpense { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public bool? ForRelease { get; set; }
        public bool? IsReleased { get; set; }
        public string Remarks { get; set; }
    }

}
