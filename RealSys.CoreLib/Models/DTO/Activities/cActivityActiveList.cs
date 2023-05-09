using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Activities
{
    public class cActivityActiveList
    {
        public string SalesCode { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public string ActivityType { get; set; }
        public decimal Amount { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }

        public IEnumerable<string> StatusDoneList { get; set; }
        public IEnumerable<string> StatusList { get; set; }
    }
}
