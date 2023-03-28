using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.Custom
{
    public class JobVehicleService
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int Mileage { get; set; }
        public DateTime DtStart { get; set; }
        public string Particulars { get; set; }
        public string Remarks { get; set; }
        public string JsServices { get; set; }
        public int JobMainId { get; set; }
    }
}
