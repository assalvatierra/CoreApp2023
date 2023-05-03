using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Activities
{
    public class cUserPerformance
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Quotation { get; set; }
        public int Meeting { get; set; }
        public int Sales { get; set; }
        public int ProcMeeting { get; set; }
        public int Procurement { get; set; }
        public int JobOrder { get; set; }
        public decimal Amount { get; set; }
        public decimal ProcAmount { get; set; }
        public string Role { get; set; }
        public string Remarks { get; set; }
    }
}
