using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Jobs
{
    public class cActiveJobs
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public string JobDesc { get; set; }
        public string Particulars { get; set; }
        public string Service { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }
        public string Item { get; set; }
        public string DtStart { get; set; }
        public string DtEnd { get; set; }
        public string JsDate { get; set; }
        public string JsTime { get; set; }
        public string JsLocation { get; set; }
        public DateTime SORTDATE { get; set; }
        public IEnumerable<JobServiceItem> Assigned { get; set; }
    }

}
