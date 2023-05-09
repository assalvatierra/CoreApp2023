using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Activities
{
    public class cSupActivityActiveList
    {
        public int SupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int StatusId { get; set; }
        public string Activity { get; set; }
        public string ActType { get; set; }
        public decimal Amount { get; set; }
        public DateTime DtActivity { get; set; }

        public IEnumerable<string> StatusDoneList { get; set; }
        public IEnumerable<string> StatusList { get; set; }
    }
}
