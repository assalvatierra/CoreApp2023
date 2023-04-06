using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.ItemSchedule
{
    public class cItemSchedule
    {
        [Key]
        public int ItemId { get; set; }
        public int? JobMainId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime? DtStart { get; set; }
        public DateTime? DtEnd { get; set; }
    }
}
