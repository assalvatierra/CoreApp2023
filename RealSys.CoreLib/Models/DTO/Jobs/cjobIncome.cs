using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Jobs
{
    public class cjobIncome
    {
        public int Id { get; set; }
        public decimal Tour { get; set; }
        public decimal Car { get; set; }
        public decimal Others { get; set; }
    }
}
