using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Activities
{
    public class cUserPerformanceReport
    {
        public string User { get; set; }
        public int Sales { get; set; }
        public int Quotation { get; set; }
        public int Meeting { get; set; }
        public int Procurement { get; set; }
        public int CallsAndEmail { get; set; }
        public int Close { get; set; }
        public string Remarks { get; set; }
    }
}
