using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.ItemSchedule
{
    public class ItemSchedule
    {
        [Key]
        public int ItemId { get; set; }
        public InvItem Item { get; set; }
        public List<DayStatus> dayStatus { get; set; }
    }
}
