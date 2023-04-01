using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.ItemSchedule
{
    public class DayStatus
    {
        [Key]
        public int Day { get; set; }
        public int Hour { get; set; }
        public DateTime Date { get; set; }
        public int status { get; set; }
        public int jobcount { get; set; }
        public List<JobServices> svc { get; set; }
    }
}
