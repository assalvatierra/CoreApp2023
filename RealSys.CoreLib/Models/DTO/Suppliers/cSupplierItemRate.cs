using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Suppliers
{
    public class cSupplierItemRate
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int InvItemId { get; set; }
        public int ItemRateId { get; set; }
        public int SupplierInvItemId { get; set; }
        public string Remarks { get; set; }
        public string DtEntered { get; set; }
        public string DtValidFrom { get; set; }
        public string DtValidTo { get; set; }
        public string Particulars { get; set; }
        public string By { get; set; }
        public string Materials { get; set; }
        public string ProcBy { get; set; }
        public string TradeTerm { get; set; }
        public string Tolerance { get; set; }

    }
}
