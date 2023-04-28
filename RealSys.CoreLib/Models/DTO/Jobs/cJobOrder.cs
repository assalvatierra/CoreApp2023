using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Jobs
{
    public class cJobOrder
    {

        [Key]
        public int Id { get; set; }
        public CoreLib.Models.Erp.JobMain Main { get; set; }
        public List<cJobService> Services { get; set; }
        public List<cjobCounter> ActionCounter { get; set; }
        public cjobIncome PostedIncome { get; set; }
        public decimal Payment { get; set; }
        public decimal Expenses { get; set; }
        public string Company { get; set; }
        public bool isPosted { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
    }
}
