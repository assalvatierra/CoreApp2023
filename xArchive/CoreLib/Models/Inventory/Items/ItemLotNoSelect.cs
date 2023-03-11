using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreLib.Inventory.Models.Items
{
    public class ItemLotNoSelect
    {
        public int Id { get; set; }
        public int LotNo { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Uom { get; set; }
        public string Status { get; set; }
        public int Qty { get; set; }
        public ICollection<InvWarningLevel> InvWarningLevels { get; set; }
    }
}