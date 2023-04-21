using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Jobs
{
    public class cjobUnitIncome
    {
        public int Id { get; set; }
        public DateTime JobDate { get; set; }
        public List<cUnitList> Unit { get; set; }
        public string Description { get; set; }
        public decimal Quoted { get; set; }
        public decimal Collected { get; set; }
        public decimal Payment { get; set; }
        public decimal Expenses { get; set; }
        public decimal Tour { get; set; }
        public decimal Car { get; set; }
        public decimal Others { get; set; }
        public bool isPosted { get; set; }
    }
}
