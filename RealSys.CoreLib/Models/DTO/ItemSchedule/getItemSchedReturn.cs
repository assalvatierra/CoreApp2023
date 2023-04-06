using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.ItemSchedule
{
    public class getItemSchedReturn
    {
        public List<ItemSchedule> ItemSched { get; set; }
        public List<DayLabel> dLabel { get; set; }
    }
}
