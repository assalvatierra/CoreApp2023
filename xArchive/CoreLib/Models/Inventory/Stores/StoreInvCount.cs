using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLib.Inventory.Models.Stores
{
    public class StoreInvCount
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Uom { get; set; }
        public string Item { get; set; }
        public string Code { get; set; }
        public decimal Available { get; set; }
        public decimal OnHand { get; set; }
        public decimal ReceivePending { get; set; }
        public decimal ReceiveAccepted { get; set; }
        public decimal ReleaseRequest { get; set; }
        public decimal ReleaseReleased { get; set; }
        public decimal Adjustments { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string ItemSpec { get; set; }

        public ICollection<InvWarningLevel> InvWarningLevels { get; set; }
    }

}
