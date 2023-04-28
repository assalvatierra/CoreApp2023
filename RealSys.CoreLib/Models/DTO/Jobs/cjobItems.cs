using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Jobs
{
    public class cjobItems
    {
        public int Id { get; set; }
        public string itemCode { get; set; }
        public string Name { get; set; }
        public string icon { get; set; }
        public string remarks { get; set; }
        public int orderNo { get; set; }
    }
}
